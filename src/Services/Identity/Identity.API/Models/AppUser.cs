using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public virtual ICollection<UserRole> userRoles { get; set; }
    }
}