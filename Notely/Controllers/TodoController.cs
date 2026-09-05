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
public class TodoController(TodoManager _manager, CoursManager _coursManager, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<TodoDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoDTO>>> GetAll()
    {
        var list = await _manager.GetAllByCompteAsync(User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<TodoDTO>>(list));
    }

    [HttpGet("{idCours}")]
    [ActionName("GetByCours")]
    [ProducesResponseType(typeof(IEnumerable<TodoDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoDTO>>> GetByCours(int idCours)
    {
        var list = await _manager.GetAllByCoursAsync(idCours, User.GetIdCompte());
        return Ok(_mapper.Map<IEnumerable<TodoDTO>>(list));
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    [ProducesResponseType(typeof(TodoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoDTO>> GetById(int id)
    {
        var entity = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (entity is null)
            return NotFound();

        return Ok(_mapper.Map<TodoDTO>(entity));
    }

    [HttpPost]
    [ActionName("Post")]
    [ProducesResponseType(typeof(TodoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoDTO>> Post([FromBody] TodoCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var idCompte = User.GetIdCompte();
        var cours = await _coursManager.GetByIdForCompteAsync(dto.IdCours, idCompte);
        if (cours is null)
            return BadRequest(new { message = "Cours introuvable." });

        var entity = _mapper.Map<Todo>(dto);
        entity.IdCompte = idCompte;

        await _manager.AddAsync(entity);

        var created = await _manager.GetByIdForCompteAsync(entity.IdTodo, idCompte);
        return CreatedAtAction(nameof(GetById), new { id = entity.IdTodo }, _mapper.Map<TodoDTO>(created));
    }

    [HttpPut("{id}")]
    [ActionName("Put")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] TodoUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _manager.GetByIdForCompteAsync(id, User.GetIdCompte());
        if (existing is null)
            return NotFound();

        var updated = _mapper.Map<Todo>(dto);
        updated.IdCours = existing.IdCours;
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
