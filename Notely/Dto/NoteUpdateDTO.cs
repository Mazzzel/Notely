using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class NoteUpdateDTO
{
    [Required]
    [MaxLength(500)]
    public string Texte { get; set; } = null!;

    public bool Fait { get; set; }
}
