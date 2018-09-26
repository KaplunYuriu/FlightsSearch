using System.Collections.Generic;
using FlightsSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : Controller
    {
        public ActionResult<IEnumerable<Airport>> Index()
        {
            return new[]
            {
                new Airport
                {
                    Alias = "Test",
                    Altitude = 12,
                    City = "Poltava",
                    Country = "UK",
                    Latitude = 12,
                    Longitude = 12,
                    Name = "Name"
                }
            };
        }
    }
}