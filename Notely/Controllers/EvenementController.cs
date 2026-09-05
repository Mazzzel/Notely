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
public class EvenementController(EvenementManager _manager, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<EvenementDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EvenementDTO>>> GetAll()
    {
        var list = await _manager.GetAllByCompteAsync(User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<EvenementDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(EvenementDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvenementDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<EvenementDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(EvenementDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EvenementDTO>> Post([FromBody] EvenementCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<Evenement>(dto);
        entity.IdCompte = User.GetIdCompte();

        await _manager.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.IdEvenement }, _mapper.Map<EvenementDTO>(entity));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] EvenementUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Evenement>(dto);
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
