using System.Security.Claims;

namespace Notely.Auth;

public static class ClaimsPrincipalExtensions
{
    public static int GetIdCompte(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("idCompte")
            ?? throw new InvalidOperationException("Le claim 'idCompte' est manquant.");
        return int.Parse(claim.Value);
    }
}
