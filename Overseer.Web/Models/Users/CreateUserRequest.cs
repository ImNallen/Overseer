using System.ComponentModel.DataAnnotations;

namespace Overseer.Web.Models.Users;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Firstname is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lastname is required")]
    public string LastName { get; set; } = string.Empty;
}
