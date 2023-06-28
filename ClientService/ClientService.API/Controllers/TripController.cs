using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    public class TripController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
