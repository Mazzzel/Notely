using System.ComponentModel.DataAnnotations;
using Notely.Entities;

namespace Notely.Dto;

public class EvenementCreateDTO : IValidatableObject
{
    [Required]
    [RegularExpression(DomaineConstantes.TypeEvenementPattern)]
    public string Type { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Titre { get; set; } = null!;

    [Required]
    [RegularExpression(DomaineConstantes.CouleurPattern)]
    public string Couleur { get; set; } = null!;

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly HeureDebut { get; set; }

    [Required]
    public TimeOnly HeureFin { get; set; }

    [MaxLength(500)]
    public string? Commentaire { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HeureFin <= HeureDebut)
            yield return new ValidationResult(
                "L'heure de fin doit être après l'heure de début.",
                new[] { nameof(HeureFin) });
    }
}
