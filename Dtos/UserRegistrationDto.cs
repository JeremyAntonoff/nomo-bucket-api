using System;
using System.ComponentModel.DataAnnotations;

namespace NomoBucket.API.Dtos
{
  public class UserRegistrationDto
  
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 characters")]
    public string Password { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public DateTime LastActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserRegistrationDto() 
    {
        LastActive = DateTime.Now;
        CreatedAt = DateTime.Now;
    }


    
  }
}