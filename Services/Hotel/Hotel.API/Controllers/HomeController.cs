using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    public class HomeController
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}