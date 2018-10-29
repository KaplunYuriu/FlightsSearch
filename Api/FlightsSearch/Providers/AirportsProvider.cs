﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;
using FlightsSearch.Infrastructure.Caching;

namespace FlightsSearch.Providers
{
    public interface IAirportsProvider
    {
        Task<Airport> GetAirport(string alias);
    }

    public class AirportsProvider : DictionaryLruCache<string, Airport>, IAirportsProvider
    {
        private readonly IApullateFlightsApi _flightsApi;

        public AirportsProvider(IApullateFlightsApi flightsApi) : base(TimeSpan.FromMinutes(30))
        {
            _flightsApi = flightsApi;
        }

        public async Task<Airport> GetAirport(string alias)
        {
            if (!CachedResource.TryGetValue(alias, out var cacheElement) || cacheElement.IsExpired(ElementLifetime))
            {
                var airports = await _flightsApi.GetAirports(alias);

                if (!airports.Any())
                    return null;

                if (airports.Count > 1)
                {
                    var airportWithCorrectAlias = airports.FirstOrDefault(a => string.Equals(a.Alias, alias));

                    if (airportWithCorrectAlias == null)
                        throw new ArgumentException($"More than one airport found for alias: {alias}");

                    CachedResource[alias] = new DictionaryCacheElement<Airport>(airportWithCorrectAlias);
                }
                else
                {
                    CachedResource[alias] = new DictionaryCacheElement<Airport>(airports.First());
                }
            }

            return CachedResource[alias].Value;
        }
    }
}