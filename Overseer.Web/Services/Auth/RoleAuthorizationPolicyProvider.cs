using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Overseer.Web.Services.Auth;

public class RoleAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public RoleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Role:", StringComparison.OrdinalIgnoreCase))
        {
            string role = policyName.Substring("Role:".Length);
            var policy = new AuthorizationPolicyBuilder();
            policy.RequireRole(role);
            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return base.GetPolicyAsync(policyName);
    }
}
