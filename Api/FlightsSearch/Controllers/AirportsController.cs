using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : Controller
    {
        private readonly IAirportsService _airportsService;

        public AirportsController(IAirportsService airportsService)
        {
            _airportsService = airportsService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Airport>>> Search([FromQuery]string pattern)
        {
            return await _airportsService.GetAirportsAsync(pattern);
        }
    }
}