using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_cours_cou")]
public class Cours
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cou_id")]
    public int IdCours { get; set; }

    [Column("cou_nom")]
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;

    [Column("cou_date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.Cours))]
    public virtual Compte CompteNav { get; set; } = null!;

    [InverseProperty(nameof(Chapitre.CoursNav))]
    public virtual ICollection<Chapitre> Chapitres { get; set; } = new List<Chapitre>();

    [InverseProperty(nameof(Todo.CoursNav))]
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
