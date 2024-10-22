using System.ComponentModel.DataAnnotations;

namespace Overseer.Web.Models.Customers;

public class CreateCustomerRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required")]
    [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zip code is required")]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zip code")]
    public string ZipCode { get; set; } = string.Empty;
}
