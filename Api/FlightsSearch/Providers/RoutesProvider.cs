using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;
using FlightsSearch.Infrastructure;
using FlightsSearch.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore.Internal;

namespace FlightsSearch.Providers
{
    public interface IRoutesProvider
    {
        Task<List<Route>> GetRoutes(Airport airport);
    }

    public class RoutesProvider : DictionaryLruCache<Airport, List<Route>>, IRoutesProvider
    {
        private readonly IApullateFlightsApi _flightsApi;
        private readonly IAirportsProvider _airportsProvider;

        public RoutesProvider(IApullateFlightsApi apullateFlightsApi, IAirportsProvider airportsProvider) : base(TimeSpan.FromMinutes(10))
        {
            _flightsApi = apullateFlightsApi;
            _airportsProvider = airportsProvider;
        }

        public async Task<List<Route>> GetRoutes(Airport airport)
        {
            var cacheElement = this[airport];

            if (cacheElement == null)
            {
                var plainRoutes = await FailSafe.TryTwice(() => _flightsApi.GetRoutes(airport.Alias));

                var convertedPlainRoutes = await Task.WhenAll(plainRoutes.Select(async r => await ConvertToRoute(r)));

                var airlines = await GetAirlinesForRoutes(convertedPlainRoutes.ToList());

                var airlinesDictionary = airlines.ToDictionary(x => x.Alias, x => x);
                foreach (var route in convertedPlainRoutes)
                {
                    route.Airline = airlinesDictionary[route.Airline.Alias];
                }

                this[airport] = convertedPlainRoutes.ToList();
            }

            return this[airport];
        }

        private async Task<Route> ConvertToRoute(PlainRoute plainRoute)
        {
            return new Route
            {
                Departure = await _airportsProvider.GetAirport(plainRoute.SrcAirport),
                Destination = await _airportsProvider.GetAirport(plainRoute.DestAirport),
                Airline = new Airline
                {
                    Alias = plainRoute.Airline
                }
            };
        }

        private async Task<Airline[]> GetAirlinesForRoutes(List<Route> routes)
        {
            var distinctRoutesByAirlines = routes.Distinct((first, second) =>
                string.Equals(first.Airline.Alias, second.Airline.Alias, StringComparison.OrdinalIgnoreCase)).ToList();

            var airlines = await Task.WhenAll(distinctRoutesByAirlines.Select(async r =>
            {
                var availableAirlines = await _flightsApi.GetAirlines(r.Airline.Alias);

                return availableAirlines[0];
            }));

            return airlines;
        }
    }
}