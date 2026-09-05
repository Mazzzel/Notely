using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class SeanceUpdateDTO
{
    [Required]
    public DateOnly Date { get; set; }

    [MaxLength(500)]
    public string? Commentaire { get; set; }
}
