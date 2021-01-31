using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Microservices.Core
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

#pragma warning disable 1998
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
#pragma warning restore 1998
        {
            if (context.User == null)
            {
                return;
            }

            var claims = context.User.Claims;
            var permission = claims
                .Where(x => (x.Type == CustomClaimsTypes.Permission &&
                             x.Value == requirement.Permission &&
                             x.Issuer == "LOCAL AUTHORITY") || 
                             x.Type== ClaimTypes.Role && 
                             x.Value== "Admin")
                .Select(x => x.Value);

            if (permission.Any())
            {
                context.Succeed(requirement);
            }
        }
    }
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // There can only be one policy provider in ASP.NET Core.
            // We only handle permissions related policies, for the rest
            /// we will use the default provider.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Dynamically creates a policy with a requirement that contains the permission.
        // The policy name must match the permission that is needed.
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permissions", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }

            // Policy is not for permissions, try the default provider.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
