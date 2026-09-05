using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class CoursCreateDTO
{
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;
}
