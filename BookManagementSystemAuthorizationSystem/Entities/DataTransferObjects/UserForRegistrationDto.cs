using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class UserForRegistrationDto
{
    public enum UserRole
    {
        User,
        Administrator
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
}
