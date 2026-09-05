namespace Notely.Dto;

public class CoursDTO
{
    public int IdCours { get; set; }
    public string Nom { get; set; } = null!;
    public DateTime DateCreation { get; set; }
    public int NombreChapitres { get; set; }
    public int NombreChapitresAppris { get; set; }
    public int NombreTachesOuvertes { get; set; }
}
