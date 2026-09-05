using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notely.Auth;
using Notely.Dto.Auth;
using Notely.Managers;
using Notely.Services;

namespace Notely.Controllers;

/// <summary>
/// Authentification. Les comptes sont créés directement en base avec un mot de passe
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

        SetAuthCookie(_jwt.GenerateToken(compte));

        return Ok(_mapper.Map<LoginResponseDTO>(compte));
    }

    [HttpPost]
    [Authorize]
    [ActionName("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        SetAuthCookie(_jwt.GenerateToken(compte));

        return NoContent();
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
        var isHttps = Request.IsHttps;
        Response.Cookies.Delete("access_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax
        });
        return NoContent();
    }

    private void SetAuthCookie(string token)
    {
        var isHttps = Request.IsHttps;

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }
}
