namespace Notely.Dto.Auth;

public class CompteDTO
{
    public int IdCompte { get; set; }
    public string Email { get; set; } = null!;
    public bool DoitChangerMotDePasse { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateDerniereConnexion { get; set; }
}
