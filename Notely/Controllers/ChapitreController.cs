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
[Authorize(Policy = Policies.PageCours)]
public class ChapitreController(ChapitreManager _manager, CoursManager _coursManager, IMapper _mapper) : ControllerBase
{
    [HttpGet("{idCours}")]
    [ActionName("GetByCours")]
    [ProducesResponseType(typeof(IEnumerable<ChapitreDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ChapitreDTO>>> GetByCours(int idCours)
    {
        var cours = await _coursManager.GetByIdForCompteAsync(idCours, User.GetIdCompte());
        if (cours is null)
            return NotFound();

        var list = await _manager.GetAllByCoursAsync(idCours);
        return Ok(_mapper.Map<IEnumerable<ChapitreDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(ChapitreDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChapitreDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<ChapitreDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(ChapitreDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChapitreDTO>> Post([FromBody] ChapitreCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cours = await _coursManager.GetByIdForCompteAsync(dto.IdCours, User.GetIdCompte());
        if (cours is null)
            return BadRequest(new { message = "Cours introuvable." });

        var entity = _mapper.Map<Chapitre>(dto);
        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdChapitre }, _mapper.Map<ChapitreDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] ChapitreUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Chapitre>(dto);
        updated.IdCours = existing.IdCours;

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
