using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class TodoCreateDTO
{
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;

    [Required]
    public int IdCours { get; set; }

    public DateOnly? Date { get; set; }
}
