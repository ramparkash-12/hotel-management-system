using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Identity.API.Controllers
{
    public class HomeController : Controller
    {

        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger _logger;

        public HomeController(IIdentityServerInteractionService interaction, ILogger<HomeController> logger)
        {
            _interaction = interaction;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var content = from c in User.Claims select new { c.Type, c. Value };
            
            ViewBag.Json = content;
            
            return View();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

            }

            return View("Error", vm);
        }
    }
}