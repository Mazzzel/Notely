namespace Notely.Entities;

public static class DomaineConstantes
{
    public const string EtatChapitrePattern = "^(non_appris|en_cours|appris)$";
    public const string DifficultePattern = "^(facile|moyen|difficile)$";
    public const string TypeEvenementPattern = "^(cours|examen|salle)$";
    public const string CouleurPattern = "^#[0-9a-fA-F]{6}$";
    public const string CodePagePattern = "^(cours|salle)$";
}
