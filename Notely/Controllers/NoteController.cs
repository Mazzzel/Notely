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
public class NoteController(NoteManager _manager, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<NoteDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NoteDTO>>> GetAll()
    {
        var list = await _manager.GetAllByCompteAsync(User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<NoteDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(NoteDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NoteDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<NoteDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(NoteDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NoteDTO>> Post([FromBody] NoteCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<Note>(dto);
        entity.IdCompte = User.GetIdCompte();

        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdNote }, _mapper.Map<NoteDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] NoteUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Note>(dto);
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
