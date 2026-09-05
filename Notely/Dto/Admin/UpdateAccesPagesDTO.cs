using System.ComponentModel.DataAnnotations;

namespace Notely.Dto.Admin;

public class UpdateAccesPagesDTO
{
    [Required]
    public List<string> Pages { get; set; } = new();
}
