using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_serie_ser")]
public class Serie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ser_id")]
    public int IdSerie { get; set; }

    [Column("ser_numero")]
    [Required]
    public int NumeroSerie { get; set; }

    [Column("ser_reps")]
    [Required]
    public int NombreReps { get; set; }

    [Column("ser_poids", TypeName = "numeric(6,2)")]
    public decimal? Poids { get; set; }

    [Column("exs_id")]
    [Required]
    public int IdExerciceSeance { get; set; }

    [ForeignKey(nameof(IdExerciceSeance))]
    [InverseProperty(nameof(Entities.ExerciceSeance.Series))]
    public virtual ExerciceSeance ExerciceSeanceNav { get; set; } = null!;
}
