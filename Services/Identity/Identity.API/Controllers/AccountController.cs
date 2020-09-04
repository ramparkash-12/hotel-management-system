using System;
using Microsoft.AspNetCore.Mvc;
using Identity.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using AutoMapper;
using Identity.API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Identity.API.Data;

namespace Identity.API.Controllers
{
  [ApiController]
  
  [Route("api/[controller]")]
  public class AccountController : Controller
  {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext context, IConfiguration configuration)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _context = context;
      _mapper = mapper;
      _configuration = configuration;
    }

    //
    // POST: /Account/Login
    /// <summary>
    /// Action to Login
    /// </summary>
    /// <param name="LoginDto">Model to login</param>
    /// <returns>Returns token and user</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
      try
      {

        var appUser = await _userManager.FindByNameAsync(loginDto.Username);

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

        if (result.Succeeded)
        {
          var userToReturn = _mapper.Map<LoginDetailsDto>(appUser);

          return Ok(new
          {
            token = await GenerateJwtToken(appUser),
            user = userToReturn
          });
        }
        else
        {
          return Unauthorized("username or password is incorrect!");
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Computer says no - " + ex);
      }
    }

    private async Task<string> GenerateJwtToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName.ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(10),
            SigningCredentials = creds,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
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
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
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
      }
      else
      {
        return BadRequest(ModelState);
      }

      return Ok("You have successfully Registered....");
    }


    private string AddModelErrors(IdentityResult result)
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