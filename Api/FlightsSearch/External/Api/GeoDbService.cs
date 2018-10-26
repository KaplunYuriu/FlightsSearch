using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using Refit;

namespace FlightsSearch.External.Api
{

    public class GeoDbResponse<T>
    {
        public T Data { get; set; }
    }

    public interface IGeoDbService
    {
        [Get("/cities")]
        Task<GeoDbResponse<List<Location>>> GetCities([AliasAs("namePrefix")]string pattern);

        [Get("/cities/{cityId}/")]

        Task<GeoDbResponse<Location>> GetCityDetails([AliasAs("cityId")]int id);
    }
}