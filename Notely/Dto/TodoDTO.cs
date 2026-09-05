namespace Notely.Dto;

public class TodoDTO
{
    public int IdTodo { get; set; }
    public string Nom { get; set; } = null!;
    public int IdCours { get; set; }
    public string NomCours { get; set; } = null!;
    public bool Fait { get; set; }
    public DateOnly? Date { get; set; }
}
