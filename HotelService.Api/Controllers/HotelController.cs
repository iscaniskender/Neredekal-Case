using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers
{
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
