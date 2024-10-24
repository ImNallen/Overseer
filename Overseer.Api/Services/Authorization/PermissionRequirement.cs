using Microsoft.AspNetCore.Authorization;

namespace Overseer.Api.Services.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission) => Permission = permission;

    public string Permission { get; }
}
