namespace Notely.Dto;

public class NoteDTO
{
    public int IdNote { get; set; }
    public string Texte { get; set; } = null!;
    public bool Fait { get; set; }
}
