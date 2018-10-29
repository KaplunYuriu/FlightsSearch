using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Location>>> Search([FromQuery]string pattern)
        {
            return await _citiesService.GetCitiesAsync(pattern);
        }
    }
}