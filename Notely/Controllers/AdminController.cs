using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notely.Auth;
using Notely.Dto.Admin;
using Notely.Managers;

namespace Notely.Controllers;

/// <summary>
/// Réservé aux comptes admin (champ com_est_admin, modifiable uniquement en base). Permet de
/// lister les comptes et de choisir, par compte, les pages auxquelles il a accès.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Policy = Policies.Admin)]
public class AdminController(CompteManager _compteManager, CompteAccesPageManager _accesManager, IMapper _mapper) : ControllerBase
{
    private static readonly string[] CodesPageValides = ["cours", "salle"];

    [HttpGet]
    [ActionName("GetComptes")]
    [ProducesResponseType(typeof(IEnumerable<CompteAdminDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CompteAdminDTO>>> GetComptes()
    {
        var comptes = await _compteManager.GetAllWithAccesPagesAsync();
        return Ok(_mapper.Map<IEnumerable<CompteAdminDTO>>(comptes));
    }

    [HttpPut("{idCompte}")]
    [ActionName("SetPages")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPages(int idCompte, [FromBody] UpdateAccesPagesDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var compte = await _compteManager.GetByIdAsync(idCompte);
        if (compte is null)
            return NotFound();

        if (dto.Pages.Any(p => !CodesPageValides.Contains(p)))
            return BadRequest(new { message = "Code de page invalide." });

        await _accesManager.SetForCompteAsync(idCompte, dto.Pages.Distinct());
        return NoContent();
    }
}
