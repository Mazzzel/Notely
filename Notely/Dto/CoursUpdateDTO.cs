using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class CoursUpdateDTO
{
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;
}
