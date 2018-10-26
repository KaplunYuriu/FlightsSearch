using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;

namespace FlightsSearch.Services
{
    public interface ICitiesService
    {
        Task<List<Location>> GetCitiesAsync(string pattern);

        Task<Location> GetCityDetails(int cityId);
    }

    public class CitiesService : ICitiesService
    {
        private readonly IGeoDbService _geoDbService;

        public CitiesService(IGeoDbService geoDbService)
        {
            _geoDbService = geoDbService;
        }

        public async Task<List<Location>> GetCitiesAsync(string pattern)
        {
            var result = await _geoDbService.GetCities(pattern);

            return result.Data;
        }

        public async Task<Location> GetCityDetails(int cityId)
        {
            var result = await _geoDbService.GetCityDetails(cityId);

            return result.Data;
        }
    }
}
