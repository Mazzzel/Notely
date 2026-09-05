using System.ComponentModel.DataAnnotations;
using Notely.Entities;

namespace Notely.Dto;

public class ChapitreCreateDTO
{
    [Required]
    public int IdCours { get; set; }

    [Required]
    [MaxLength(200)]
    public string Libelle { get; set; } = null!;

    [RegularExpression(DomaineConstantes.EtatChapitrePattern)]
    public string Etat { get; set; } = "non_appris";

    public DateOnly? Date { get; set; }

    [RegularExpression(DomaineConstantes.DifficultePattern)]
    public string Difficulte { get; set; } = "moyen";
}
