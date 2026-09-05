using System.ComponentModel.DataAnnotations;

namespace Notely.Dto.Auth;

public class ChangePasswordRequestDTO
{
    [Required]
    public string MotDePasseActuel { get; set; } = null!;

    [Required]
    [MinLength(8, ErrorMessage = "Le nouveau mot de passe doit contenir au moins 8 caractères.")]
    public string NouveauMotDePasse { get; set; } = null!;
}
