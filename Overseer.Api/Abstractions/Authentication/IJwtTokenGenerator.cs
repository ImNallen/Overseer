using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users;

namespace Overseer.Api.Services.Authentication;

public interface IJwtTokenGenerator
{
    Token GenerateToken(User user);
}
