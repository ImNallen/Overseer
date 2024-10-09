using Microsoft.AspNetCore.Authorization;

namespace Overseer.Web.Services.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class RequireRoleAttribute : AuthorizeAttribute
{
    public RequireRoleAttribute(string role)
    {
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Policy = $"Role:{role}";
    }

    public string Role { get; }
}
