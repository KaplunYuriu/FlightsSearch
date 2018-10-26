using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;
using FlightsSearch.Infrastructure;

namespace FlightsSearch.Services
{
    public interface IAirportsService
    {
        Task<List<Airport>> GetAirportsAsync(string pattern);
        Task<SortedList<double, Airport>> GetClosestToCity(int cityId);
    }

    public class AirportsService : IAirportsService
    {
        private readonly IApullateFlightsApi _flightsApi;
        private readonly ICitiesService _citiesService;

        public AirportsService(IApullateFlightsApi flightsApi, ICitiesService citiesService)
        {
            _flightsApi = flightsApi;
            _citiesService = citiesService;
        }

        public async Task<List<Airport>> GetAirportsAsync(string pattern)
        {
            var result = await _flightsApi.GetAirports(pattern);

            return result;
        }

        public async Task<SortedList<double, Airport>> GetClosestToCity(int cityId)
        {
            var city = await _citiesService.GetCityDetails(cityId);

            if (city == null)
                throw new ArgumentNullException(nameof(city), $"City with id {cityId} doesn`t exist");

            var airports = await _flightsApi.GetAirports(city.Country);

            var airportsByDistance = new SortedList<double, Airport>(airports.Count, new DescComparer<double>());
            foreach (var airport in airports)
            {
                double distanceToCity = DistanceCalculator.GetDistance(city, airport);

                airportsByDistance.Add(distanceToCity, airport);
            }

            return airportsByDistance;
        }
    }
}