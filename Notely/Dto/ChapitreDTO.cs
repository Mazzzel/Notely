namespace Notely.Dto;

public class ChapitreDTO
{
    public int IdChapitre { get; set; }
    public int IdCours { get; set; }
    public string Libelle { get; set; } = null!;
    public string Etat { get; set; } = null!;
    public DateOnly? Date { get; set; }
    public string Difficulte { get; set; } = null!;
}
