using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notely.Entities;

[Table("t_e_compte_com")]
public class Compte
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("com_id")]
    public int IdCompte { get; set; }

    [Column("com_email")]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Column("com_mdp_hash")]
    [Required]
    [MaxLength(64)]
    public string MotDePasseHash { get; set; } = null!;

    [Column("com_doit_changer_mdp")]
    public bool DoitChangerMotDePasse { get; set; } = true;

    [Column("com_date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [Column("com_date_derniere_connexion")]
    public DateTime? DateDerniereConnexion { get; set; }

    [Column("com_est_admin")]
    public bool EstAdmin { get; set; } = false;

    [InverseProperty(nameof(Entities.CompteAccesPage.CompteNav))]
    public virtual ICollection<CompteAccesPage> AccesPages { get; set; } = new List<CompteAccesPage>();

    [InverseProperty(nameof(Entities.Cours.CompteNav))]
    public virtual ICollection<Cours> Cours { get; set; } = new List<Cours>();

    [InverseProperty(nameof(Entities.Todo.CompteNav))]
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();

    [InverseProperty(nameof(Entities.Note.CompteNav))]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    [InverseProperty(nameof(Entities.Evenement.CompteNav))]
    public virtual ICollection<Evenement> Evenements { get; set; } = new List<Evenement>();

    [InverseProperty(nameof(Entities.Seance.CompteNav))]
    public virtual ICollection<Seance> Seances { get; set; } = new List<Seance>();
}
