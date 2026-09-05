using System.Security.Cryptography;
using System.Text;

namespace Notely.Services;

public class Sha256PasswordHasher : IPasswordHasher
{
    public string Hash(string plain)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plain));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public bool Verify(string plain, string hash)
    {
        return Hash(plain) == hash.ToLowerInvariant();
    }
}
