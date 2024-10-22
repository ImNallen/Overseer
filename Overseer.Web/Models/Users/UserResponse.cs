namespace Overseer.Web.Models.Users;

public record UserResponse(
    Guid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Role,
    bool IsEmailVerified,
    DateTime CreatedAt);
