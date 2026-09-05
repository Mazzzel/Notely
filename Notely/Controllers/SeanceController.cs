using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notely.Auth;
using Notely.Dto;
using Notely.Entities;
using Notely.Managers;

namespace Notely.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Policy = Policies.Authorized)]
public class SeanceController(SeanceManager _manager, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<SeanceDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SeanceDTO>>> GetAll()
    {
        var list = await _manager.GetAllByCompteAsync(User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<SeanceDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(SeanceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SeanceDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<SeanceDTO>(entity));
    }

    [HttpGet]
    [ActionName("GetExercices")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetExercices()
    {
        return Ok(await _manager.GetNomsExercicesDistinctsAsync(User.GetIdCompte()));
    }

    [HttpGet]
    [ActionName("GetProgression")]
    [ProducesResponseType(typeof(IEnumerable<ProgressionPointDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProgressionPointDTO>>> GetProgression([FromQuery] string exercice)
    {
        if (string.IsNullOrWhiteSpace(exercice))
            return BadRequest(new { message = "Le paramètre 'exercice' est requis." });

        return Ok(await _manager.GetProgressionAsync(User.GetIdCompte(), exercice));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(SeanceDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SeanceDTO>> Post([FromBody] SeanceCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<Seance>(dto);
        entity.IdCompte = User.GetIdCompte();

        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdSeance }, _mapper.Map<SeanceDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] SeanceUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Seance>(dto);
        updated.IdCompte = existing.IdCompte;

        await _manager.UpdateAsync(existing, updated);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ActionName("Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        await _manager.DeleteAsync(existing);
        return NoContent();
    }
}
