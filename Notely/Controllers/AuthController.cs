using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notely.Auth;
using Notely.Dto.Auth;
using Notely.Managers;
using Notely.Services;

namespace Notely.Controllers;

/// <summary>
/// Authentification par token Bearer (le front stocke le token et l'envoie via l'en-tête
/// Authorization). Les comptes sont créés directement en base avec un mot de passe
/// temporaire et le flag DoitChangerMotDePasse à true : tant que ce flag est actif,
/// seules les routes de ce contrôleur restent accessibles.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(
    CompteManager _manager,
    IPasswordHasher _hasher,
    IJwtTokenService _jwt,
    IMapper _mapper) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ActionName("Login")]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var compte = await _manager.GetByEmailAsync(dto.Email);
        if (compte is null || !_hasher.Verify(dto.MotDePasse, compte.MotDePasseHash))
            return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        compte.DateDerniereConnexion = DateTime.UtcNow;
        await _manager.SaveChangesAsync();

        var response = _mapper.Map<LoginResponseDTO>(compte);
        response.Token = _jwt.GenerateToken(compte);

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    [ActionName("ChangePassword")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var compte = await _manager.GetByIdAsync(User.GetIdCompte());
        if (compte is null)
            return NotFound();

        if (!_hasher.Verify(dto.MotDePasseActuel, compte.MotDePasseHash))
            return BadRequest(new { message = "Mot de passe actuel incorrect." });

        compte.MotDePasseHash = _hasher.Hash(dto.NouveauMotDePasse);
        compte.DoitChangerMotDePasse = false;
        await _manager.SaveChangesAsync();

        return Ok(new { token = _jwt.GenerateToken(compte) });
    }

    [HttpGet]
    [Authorize]
    [ActionName("Me")]
    [ProducesResponseType(typeof(CompteDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<CompteDTO>> Me()
    {
        var compte = await _manager.GetByIdAsync(User.GetIdCompte());
        if (compte is null)
            return NotFound();

        return Ok(_mapper.Map<CompteDTO>(compte));
    }

    [HttpPost]
    [Authorize]
    [ActionName("Logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        return NoContent();
    }
}
