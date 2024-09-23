using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Overseer.Api.Services.Authorization;

public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) => _options = options.Value;

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = _options.GetPolicy(policyName);

        if (policy is not null)
        {
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        _options.AddPolicy(policyName, policy);

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => Task.FromResult(_options.DefaultPolicy);

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult(_options.FallbackPolicy);
}
