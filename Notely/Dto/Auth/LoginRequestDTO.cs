using System.ComponentModel.DataAnnotations;

namespace Notely.Dto.Auth;

public class LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string MotDePasse { get; set; } = null!;
}
