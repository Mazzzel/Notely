namespace Notely.Dto;

public class CoursDetailDTO
{
    public int IdCours { get; set; }
    public string Nom { get; set; } = null!;
    public DateTime DateCreation { get; set; }
    public List<ChapitreDTO> Chapitres { get; set; } = new();
    public List<TodoDTO> Todos { get; set; } = new();
}
