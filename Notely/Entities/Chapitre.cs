using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_chapitre_cha")]
public class Chapitre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cha_id")]
    public int IdChapitre { get; set; }

    [Column("cha_libelle")]
    [Required]
    [MaxLength(200)]
    public string Libelle { get; set; } = null!;

    [Column("cha_etat")]
    [Required]
    [MaxLength(20)]
    public string Etat { get; set; } = "non_appris";

    [Column("cha_date")]
    public DateOnly? Date { get; set; }

    [Column("cha_difficulte")]
    [Required]
    [MaxLength(20)]
    public string Difficulte { get; set; } = "moyen";

    [Column("cou_id")]
    [Required]
    public int IdCours { get; set; }

    [ForeignKey(nameof(IdCours))]
    [InverseProperty(nameof(Entities.Cours.Chapitres))]
    public virtual Cours CoursNav { get; set; } = null!;
}
