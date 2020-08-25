using System;
using Microsoft.AspNetCore.Mvc;
using Identity.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using AutoMapper;
using Identity.API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Identity.API.Data;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        
        public AccountController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }



        //
        // POST: /Account/Register
        /// <summary>
        /// Action to register customer with credentials in the database.
        /// </summary>
        /// <param name="RegisterDto">Model to create a new customer</param>
        /// <returns>Returns Succcess message</returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            if (ModelState.IsValid) 
            {
                //** Verify if email is not registered
                var userFound = await _userManager.FindByEmailAsync(registerDto.Email);

                if (userFound != null)
                    return BadRequest("This email is already registered. Please use a different email.");

                //** Register User
                var Identityuser = _mapper.Map<AppUser>(registerDto);
                var userCreated = await _userManager.CreateAsync(Identityuser, registerDto.Password);
                //** Error Handling
                if (!userCreated.Succeeded)
                { 
                    return BadRequest(AddModelErrors(userCreated));
                } 

                //** Add Role
                var roleAssigned = await _userManager.AddToRoleAsync(Identityuser, "Customer");
                //** Error Handling
                if (!roleAssigned.Succeeded)
                { 
                    return BadRequest(AddModelErrors(roleAssigned));
                } 

                //** Add Customer
                await _context.Customers.AddAsync(new Customer { IdentityId = Identityuser.Id });
                await _context.SaveChangesAsync();
            } else 
            {
                return BadRequest(ModelState);
            }

            return Ok("You have successfully Registered....");
        }

        public string AddModelErrors(IdentityResult result)
        {
            var modelErrors = new List<string>();
            foreach (var error in result.Errors)
            {
                modelErrors.Add(error.Description.ToString());
            }
            return string.Join("<br>", modelErrors);
        }


    }
}