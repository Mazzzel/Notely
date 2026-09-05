using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class TodoUpdateDTO
{
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;

    public bool Fait { get; set; }

    public DateOnly? Date { get; set; }
}
