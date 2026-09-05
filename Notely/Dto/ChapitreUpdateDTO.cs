using System.ComponentModel.DataAnnotations;
using Notely.Entities;

namespace Notely.Dto;

public class ChapitreUpdateDTO
{
    [Required]
    [MaxLength(200)]
    public string Libelle { get; set; } = null!;

    [Required]
    [RegularExpression(DomaineConstantes.EtatChapitrePattern)]
    public string Etat { get; set; } = null!;

    public DateOnly? Date { get; set; }

    [Required]
    [RegularExpression(DomaineConstantes.DifficultePattern)]
    public string Difficulte { get; set; } = null!;
}
