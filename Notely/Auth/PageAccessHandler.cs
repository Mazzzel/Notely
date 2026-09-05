using Microsoft.AspNetCore.Authorization;
using Notely.Managers;

namespace Notely.Auth;

/// <summary>
/// Vérifie l'accès à une page en base à chaque requête (pas via un claim JWT) afin qu'une
/// modification faite depuis la page admin s'applique immédiatement, sans attendre une
/// reconnexion. Un compte admin (claim "estAdmin") a toujours accès à toutes les pages.
/// </summary>
public class PageAccessHandler(CompteAccesPageManager _manager) : AuthorizationHandler<PageAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PageAccessRequirement requirement)
    {
        var adminClaim = context.User.FindFirst("estAdmin");
        if (adminClaim is not null && adminClaim.Value == "true")
        {
            context.Succeed(requirement);
            return;
        }

        var idClaim = context.User.FindFirst("idCompte");
        if (idClaim is null || !int.TryParse(idClaim.Value, out var idCompte))
            return;

        if (await _manager.HasAccessAsync(idCompte, requirement.CodePage))
            context.Succeed(requirement);
    }
}
