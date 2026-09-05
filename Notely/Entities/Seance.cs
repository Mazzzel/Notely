using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_seance_sea")]
public class Seance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("sea_id")]
    public int IdSeance { get; set; }

    [Column("sea_date")]
    [Required]
    public DateOnly Date { get; set; }

    [Column("sea_commentaire")]
    [MaxLength(500)]
    public string? Commentaire { get; set; }

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.Seances))]
    public virtual Compte CompteNav { get; set; } = null!;

    [InverseProperty(nameof(ExerciceSeance.SeanceNav))]
    public virtual ICollection<ExerciceSeance> ExercicesSeance { get; set; } = new List<ExerciceSeance>();
}
