using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_exercice_seance_exs")]
public class ExerciceSeance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("exs_id")]
    public int IdExerciceSeance { get; set; }

    [Column("exs_nom")]
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;

    [Column("sea_id")]
    [Required]
    public int IdSeance { get; set; }

    [ForeignKey(nameof(IdSeance))]
    [InverseProperty(nameof(Entities.Seance.ExercicesSeance))]
    public virtual Seance SeanceNav { get; set; } = null!;

    [InverseProperty(nameof(Serie.ExerciceSeanceNav))]
    public virtual ICollection<Serie> Series { get; set; } = new List<Serie>();
}
