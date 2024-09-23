using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Services.Authorization;

public class PermissionAuthorizationHandler(
    IUnitOfWork unitOfWork,
    ILogger<PermissionAuthorizationHandler> logger)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        logger.LogInformation("Handling permission requirement: {Permission}", requirement.Permission);

        if (context.User.Identity is not { IsAuthenticated: true })
        {
            return;
        }

        logger.LogInformation("User is authenticated: {IsAuthenticated}", context.User.Identity.IsAuthenticated);

        Guid userId = context.User.GetUserId();

        User? user = await unitOfWork.Users
            .Where(x => x.Id == userId)
            .Include(x => x.Roles)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return;
        }

        logger.LogInformation("User found: {User}", user);

        var userPermissions = user.Roles
            .SelectMany(x => x.RolePermissions)
            .Select(x => x.Permission.Name)
            .ToHashSet();

        logger.LogInformation("User permissions: {Permissions}", userPermissions);

        if (userPermissions.Contains(requirement.Permission))
        {
            logger.LogInformation("User has permission: {Permission}", requirement.Permission);

            context.Succeed(requirement);
        }
    }
}
