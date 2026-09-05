using Microsoft.AspNetCore.Authorization;

namespace Notely.Auth;

public class MustChangePasswordHandler : AuthorizationHandler<MustChangePasswordRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MustChangePasswordRequirement requirement)
    {
        var claim = context.User.FindFirst("doitChangerMdp");
        if (claim is not null && claim.Value != "true")
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
