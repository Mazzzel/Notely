using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_todo_tod")]
public class Todo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("tod_id")]
    public int IdTodo { get; set; }

    [Column("tod_nom")]
    [Required]
    [MaxLength(200)]
    public string Nom { get; set; } = null!;

    [Column("tod_fait")]
    public bool Fait { get; set; } = false;

    [Column("tod_date")]
    public DateOnly? Date { get; set; }

    [Column("cou_id")]
    [Required]
    public int IdCours { get; set; }

    [Column("com_id")]
    [Required]
    public int IdCompte { get; set; }

    [ForeignKey(nameof(IdCours))]
    [InverseProperty(nameof(Entities.Cours.Todos))]
    public virtual Cours CoursNav { get; set; } = null!;

    [ForeignKey(nameof(IdCompte))]
    [InverseProperty(nameof(Entities.Compte.Todos))]
    public virtual Compte CompteNav { get; set; } = null!;
}
