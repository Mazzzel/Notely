namespace Notely.Dto.Auth;

public class LoginResponseDTO
{
    public int IdCompte { get; set; }
    public string Email { get; set; } = null!;
    public bool DoitChangerMotDePasse { get; set; }
    public bool EstAdmin { get; set; }
    public List<string> Pages { get; set; } = new();
    public string Token { get; set; } = null!;
}
