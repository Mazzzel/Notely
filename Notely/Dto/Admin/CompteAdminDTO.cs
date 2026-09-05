namespace Notely.Dto.Admin;

public class CompteAdminDTO
{
    public int IdCompte { get; set; }
    public string Email { get; set; } = null!;
    public bool EstAdmin { get; set; }
    public List<string> Pages { get; set; } = new();
    public DateTime DateCreation { get; set; }
    public DateTime? DateDerniereConnexion { get; set; }
}
