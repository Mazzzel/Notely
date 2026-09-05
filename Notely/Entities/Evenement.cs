using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_evenement_evt")]
public class Evenement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("evt_id")]
    public int IdEvenement { get; set; }

    [Column("evt_type")]
    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = null!;

    [Column("evt_titre")]
    [Required]
    [MaxLength(200)]
    public string Titre { get; set; } = null!;

    [Column("evt_couleur")]
    [Required]
    [MaxLength(7)]
    public string Couleur { get; set; } = null!;

    [Column("evt_date")]
    [Required]
    public DateOnly Date { get; set; }

    [Column("evt_heure_debut")]
    [Required]
    public TimeOnly HeureDebut { get; set; }

    [Column("evt_heure_fin")]
    [Required]
    public TimeOnly HeureFin { get; set; }

    [Column("evt_commentaire")]
    [MaxLength(500)]
    public string? Commentaire { get; set; }

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.Evenements))]
    public virtual Compte CompteNav { get; set; } = null!;
}
