using System;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;
using FlightsSearch.Providers;
using FlightsSearch.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace FlightsSearch
{
    public static class ConfigurationProperties
    {
        public static string ApullateFlightsApiUrl = nameof(ApullateFlightsApiUrl);
        public static string GeoDbApiUrl = nameof(GeoDbApiUrl);
        public static string EmberAppUrl = nameof(EmberAppUrl);
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new RefitSettings();

            services.AddRefitClient<IApullateFlightsApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration[ConfigurationProperties.ApullateFlightsApiUrl]));

            services.AddRefitClient<IGeoDbService>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration[ConfigurationProperties.GeoDbApiUrl]));

            services.AddSingleton<IAirportsProvider, AirportsProvider>();
            services.AddSingleton<IRoutesProvider, RoutesProvider>();
            
            services.AddTransient<IAirportsService, AirportsService>();
            services.AddTransient<ICitiesService, CitiesService>();
            services.AddTransient<IRoutesService, RoutesService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            Airport.RoutesProvider = app.ApplicationServices.GetService<IRoutesProvider>();

            app.UseCors(builder =>
                builder.WithOrigins(Configuration[ConfigurationProperties.EmberAppUrl])
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
