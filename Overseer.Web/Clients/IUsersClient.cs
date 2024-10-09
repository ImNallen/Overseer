using Overseer.Web.Models.Users;
using Refit;

namespace Overseer.Web.Clients;

public interface IUsersClient
{
    [Post("/api/v1/users/login")]
    Task<TokenResponse> Login(LoginRequest request);

    [Get("/api/v1/users/profile")]
    Task<ProfileResponse> GetProfile();

    [Put("/api/v1/users/profile")]
    Task<ProfileResponse> UpdateProfile(UpdateUserRequest request);
}
