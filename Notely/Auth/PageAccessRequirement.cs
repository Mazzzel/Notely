using Microsoft.AspNetCore.Authorization;

namespace Notely.Auth;

public class PageAccessRequirement(string codePage) : IAuthorizationRequirement
{
    public string CodePage { get; } = codePage;
}
