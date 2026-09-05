using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class SeanceCreateDTO
{
    [Required]
    public DateOnly Date { get; set; }

    [MaxLength(500)]
    public string? Commentaire { get; set; }
}
