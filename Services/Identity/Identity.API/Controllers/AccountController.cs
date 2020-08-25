using System;
using Microsoft.AspNetCore.Mvc;
using Identity.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

    }
}