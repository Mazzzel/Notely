using Notely.Entities;

namespace Notely.Services;

public interface IJwtTokenService
{
    string GenerateToken(Compte compte);
}
