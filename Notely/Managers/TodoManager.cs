using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class TodoManager : WriteableReadableManager<Todo>
{
    public TodoManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Todo>> GetAllByCompteAsync(int idCompte)
    {
        return await dbSet
            .Include(t => t.CoursNav)
            .Where(t => t.IdCompte == idCompte)
            .OrderBy(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllByCoursAsync(int idCours, int idCompte)
    {
        return await dbSet
            .Include(t => t.CoursNav)
            .Where(t => t.IdCours == idCours && t.IdCompte == idCompte)
            .OrderBy(t => t.Date)
            .ToListAsync();
    }

    public async Task<Todo?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(t => t.CoursNav)
            .FirstOrDefaultAsync(t => t.IdTodo == id && t.IdCompte == idCompte);
    }
}
