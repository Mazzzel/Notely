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
public class CoursController(CoursManager _manager, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<CoursDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CoursDTO>>> GetAll()
    {
        var list = await _manager.GetAllByCompteAsync(User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<CoursDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(CoursDetailDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CoursDetailDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<CoursDetailDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(CoursDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CoursDTO>> Post([FromBody] CoursCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<Cours>(dto);
        entity.IdCompte = User.GetIdCompte();
        entity.DateCreation = DateTime.UtcNow;

        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdCours }, _mapper.Map<CoursDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] CoursUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Cours>(dto);
        updated.IdCompte = existing.IdCompte;
        updated.DateCreation = existing.DateCreation;

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
