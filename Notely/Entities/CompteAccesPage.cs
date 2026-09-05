using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_compte_page_cpa")]
public class CompteAccesPage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cpa_id")]
    public int IdCompteAccesPage { get; set; }

    [Column("cpa_code_page")]
    [Required]
    [MaxLength(20)]
    public string CodePage { get; set; } = null!;

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.AccesPages))]
    public virtual Compte CompteNav { get; set; } = null!;
}
