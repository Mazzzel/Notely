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
public class ExerciceSeanceController(ExerciceSeanceManager _manager, SeanceManager _seanceManager, IMapper _mapper) : ControllerBase
{
    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(ExerciceSeanceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciceSeanceDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<ExerciceSeanceDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(ExerciceSeanceDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExerciceSeanceDTO>> Post([FromBody] ExerciceSeanceCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var seance = await _seanceManager.GetByIdForCompteAsync(dto.IdSeance, User.GetIdCompte());
        if (seance is null)
            return BadRequest(new { message = "Séance introuvable." });

        var entity = _mapper.Map<ExerciceSeance>(dto);
        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdExerciceSeance }, _mapper.Map<ExerciceSeanceDTO>(entity));
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
