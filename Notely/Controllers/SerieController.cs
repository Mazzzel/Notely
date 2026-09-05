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
[Authorize(Policy = Policies.PageSalle)]
public class SerieController(SerieManager _manager, ExerciceSeanceManager _exerciceSeanceManager, IMapper _mapper) : ControllerBase
{
    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(SerieDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SerieDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<SerieDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(SerieDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SerieDTO>> Post([FromBody] SerieCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exercice = await _exerciceSeanceManager.GetByIdForCompteAsync(dto.IdExerciceSeance, User.GetIdCompte());
        if (exercice is null)
            return BadRequest(new { message = "Exercice introuvable." });

        var entity = _mapper.Map<Serie>(dto);
        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdSerie }, _mapper.Map<SerieDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] SerieUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Serie>(dto);
        updated.IdExerciceSeance = existing.IdExerciceSeance;

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
