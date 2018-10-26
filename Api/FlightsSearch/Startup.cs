using System;
using FlightsSearch.External.Api;
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
        public static string ApullateFlightsApiUrl = "ApullateFlightsApiUrl";
        public static string GeoDbApiUrl = "GeoDbApiUrl";
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

            services.AddSingleton<IAirportsService, AirportsService>();
            services.AddSingleton<ICitiesService, CitiesService>();

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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
