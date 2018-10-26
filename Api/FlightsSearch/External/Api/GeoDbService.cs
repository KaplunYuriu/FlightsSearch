using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using Refit;

namespace FlightsSearch.External.Api
{

    public class GeoDbResponse
    {
        public List<Location> Data { get; set; }
    }

    public interface IGeoDbService
    {
        [Get("/geo/cities")]
        Task<GeoDbResponse> GetCities([AliasAs("namePrefix")]string pattern);
    }
}