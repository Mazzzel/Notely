namespace Notely.Dto;

public class ExerciceSeanceDTO
{
    public int IdExerciceSeance { get; set; }
    public int IdSeance { get; set; }
    public string Nom { get; set; } = null!;
    public List<SerieDTO> Series { get; set; } = new();
}
