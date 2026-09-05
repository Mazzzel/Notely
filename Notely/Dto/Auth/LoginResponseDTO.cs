namespace Notely.Dto.Auth;

public class LoginResponseDTO
{
    public int IdCompte { get; set; }
    public string Email { get; set; } = null!;
    public bool DoitChangerMotDePasse { get; set; }
}
