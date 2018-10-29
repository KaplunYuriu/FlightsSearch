using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using Refit;

namespace FlightsSearch.External.Api
{
    public class PlainRoute
    {
        public string Airline { get; set;}

        public string SrcAirport { get; set; }

        public string DestAirport { get; set; }
    }

    public interface IApullateFlightsApi
    {
        [Get("/Airport/search")]
        Task<List<Airport>> GetAirports(string pattern);

        [Get("/Route/outgoing")]
        Task<List<PlainRoute>> GetRoutes([AliasAs("airport")] string srcAirport);

        [Get("/Airline/{alias}")]
        Task<List<Airline>> GetAirlines(string alias);
    }
}