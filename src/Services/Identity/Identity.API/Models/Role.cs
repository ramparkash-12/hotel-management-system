using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class Role : IdentityRole<string>
    {
        public virtual ICollection<UserRole> userRoles { get; set; } 
    }
}