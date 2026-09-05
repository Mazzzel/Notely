using Microsoft.AspNetCore.Authorization;

namespace Notely.Auth;

public class AdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminRequirement requirement)
    {
        var claim = context.User.FindFirst("estAdmin");
        if (claim is not null && claim.Value == "true")
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
