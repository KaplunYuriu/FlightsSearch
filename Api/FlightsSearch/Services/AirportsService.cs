using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.External.Api;

namespace FlightsSearch.Services
{
    public interface IAirportsService
    {
        Task<List<Airport>> GetAirportsAsync(string pattern);
    }

    public class AirportsService : IAirportsService
    {
        private readonly IApullateFlightsApi _flightsApi;

        public AirportsService(IApullateFlightsApi flightsApi)
        {
            _flightsApi = flightsApi;
        }

        public async Task<List<Airport>> GetAirportsAsync(string pattern)
        {
            var result = await _flightsApi.GetAirports(pattern);

            return result;
        }
    }
}