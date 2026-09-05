namespace Notely.Dto;

public class SeanceDTO
{
    public int IdSeance { get; set; }
    public DateOnly Date { get; set; }
    public string? Commentaire { get; set; }
    public List<ExerciceSeanceDTO> Exercices { get; set; } = new();
}
