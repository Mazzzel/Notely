using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_note_not")]
public class Note
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("not_id")]
    public int IdNote { get; set; }

    [Column("not_texte")]
    [Required]
    [MaxLength(500)]
    public string Texte { get; set; } = null!;

    [Column("not_fait")]
    public bool Fait { get; set; } = false;

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.Notes))]
    public virtual Compte CompteNav { get; set; } = null!;
}
