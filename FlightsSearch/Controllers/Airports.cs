using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    public class Airports : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}