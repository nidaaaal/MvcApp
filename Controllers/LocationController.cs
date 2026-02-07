using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers
{
    public class LocationController : Controller
    {
        [HttpGet]
        public IActionResult GetCities(string state  )
        {
            var cities = state switch
            {
                "Kerala" => new[] { "Calicut", "Kochi", "Trivandrum", "Thrissur", "Kannur" },
                "Tamilnadu" => new[] { "Chennai", "Coimbatore" },
                "Karnataka" => new[] { "Bangalore", "Mysore" },
                _ => Array.Empty<string>()
            };

            return Json(cities);
        }
    }
}
