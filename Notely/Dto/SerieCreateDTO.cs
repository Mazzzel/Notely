using System.ComponentModel.DataAnnotations;

namespace Notely.Dto;

public class SerieCreateDTO
{
    [Required]
    public int IdExerciceSeance { get; set; }

    [Required]
    [Range(1, 200)]
    public int NumeroSerie { get; set; }

    [Required]
    [Range(1, 1000)]
    public int NombreReps { get; set; }

    [Range(0, 2000)]
    public decimal? Poids { get; set; }
}
