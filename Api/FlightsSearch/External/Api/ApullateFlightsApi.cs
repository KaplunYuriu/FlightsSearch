using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using Refit;

namespace FlightsSearch.External.Api
{
    public interface IApullateFlightsApi
    {
        [Get("/Airport/search")]
        Task<List<Airport>> GetAirports(string pattern);
    }
}