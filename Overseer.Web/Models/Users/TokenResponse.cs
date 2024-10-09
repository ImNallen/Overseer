namespace Overseer.Web.Models.Users;

public record TokenResponse(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken,
    int RefreshExpiresIn);
