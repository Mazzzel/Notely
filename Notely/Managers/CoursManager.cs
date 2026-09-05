using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class CoursManager : WriteableReadableManager<Cours>
{
    public CoursManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cours>> GetAllByCompteAsync(int idCompte)
    {
        return await dbSet
            .Include(c => c.Chapitres)
            .Include(c => c.Todos)
            .Where(c => c.IdCompte == idCompte)
            .OrderBy(c => c.Nom)
            .ToListAsync();
    }

    public async Task<Cours?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(c => c.Chapitres)
            .Include(c => c.Todos)
            .FirstOrDefaultAsync(c => c.IdCours == id && c.IdCompte == idCompte);
    }
}
