namespace Notely.Dto;

public class EvenementDTO
{
    public int IdEvenement { get; set; }
    public string Type { get; set; } = null!;
    public string Titre { get; set; } = null!;
    public string Couleur { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public string? Commentaire { get; set; }
}
