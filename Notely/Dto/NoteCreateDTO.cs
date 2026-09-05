using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class NoteCreateDTO
{
    [Required]
    [MaxLength(500)]
    public string Texte { get; set; } = null!;
}
