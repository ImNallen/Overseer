using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Services.Authorization;

public class PermissionAuthorizationHandler(
    IUnitOfWork unitOfWork)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            return;
        }

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

        var userPermissions = user.Roles
            .SelectMany(x => x.RolePermissions)
            .Select(x => x.Permission.Name)
            .ToHashSet();

        if (userPermissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
