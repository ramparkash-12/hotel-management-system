using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
  public class EFLoginService : ILoginService<AppUser>
  {
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private ApplicationDbContext _context;

    public EFLoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _context = context;
    }

    public async Task<AppUser> FindByUsername(string user)
    {
      return await _userManager.FindByEmailAsync(user);
    }

    public async Task<IdentityResult> CreateUser(AppUser user, string password)
    {
      return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> ValidateCredentials(AppUser user, string password)
    {
      return await _userManager.CheckPasswordAsync(user, password);
    }

    public Task SignIn(AppUser user)
    {
      return _signInManager.SignInAsync(user, true);
    }

    public Task SignInAsync(AppUser user, AuthenticationProperties properties, string authenticationMethod = null)
    {
      try
      {
        return _signInManager.SignInAsync(user, properties, authenticationMethod);
      }
      catch(Exception ex)
      {
        throw ex;
      }
    }
  }
}