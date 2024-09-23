using System.Security.Claims;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Features.Users;

public record ProfileQuery(Guid UserId) : IQuery<User>;

public record ProfileResponse(string Email, string FirstName, string LastName);

public class ProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/users/profile", async (
            ClaimsPrincipal claims,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new ProfileQuery(claims.GetUserId());

            Result<User> result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(new ProfileResponse(result.Value.Email, result.Value.FirstName, result.Value.LastName));
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.UsersRead);
}

public class ProfileHandler(
    IUnitOfWork unitOfWork)
    : IQueryHandler<ProfileQuery, User>
{
    public async Task<Result<User>> Handle(ProfileQuery request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<User>(UserErrors.NotFound);
        }

        return user;
    }
}
