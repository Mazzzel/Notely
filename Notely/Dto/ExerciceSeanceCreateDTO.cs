using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class ExerciceSeanceCreateDTO
{
    [Required]
    public int IdSeance { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;
}
