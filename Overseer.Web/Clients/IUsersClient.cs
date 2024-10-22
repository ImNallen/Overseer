using Overseer.Web.Models;
using Overseer.Web.Models.Users;
using Refit;

namespace Overseer.Web.Clients;

public interface IUsersClient
{
    [Get("/api/v1/users")]
    Task<PagedList<UserResponse>> GetUsers(
        [Query] string? searchTerm = null,
        [Query] string? sortColumn = null,
        [Query] string? sortOrder = null,
        [Query] int page = 1,
        [Query] int pageSize = 10);

    [Post("/api/v1/users/login")]
    Task<TokenResponse> Login(LoginRequest request);

    [Get("/api/v1/users/profile")]
    Task<ProfileResponse> GetProfile();

    [Put("/api/v1/users/profile")]
    Task<ProfileResponse> UpdateProfile(UpdateUserRequest request);
}
