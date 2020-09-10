using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Dtos
{
  public class ProfileDto
  {
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public string Gender { get; set; }
  }
}