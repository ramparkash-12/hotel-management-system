using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Boolean hasPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "You must specify a password of minimum 6 characters")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }        
    }
}