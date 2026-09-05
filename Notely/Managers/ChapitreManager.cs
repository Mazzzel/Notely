using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class ChapitreManager : WriteableReadableManager<Chapitre>
{
    public ChapitreManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Chapitre>> GetAllByCoursAsync(int idCours)
    {
        return await dbSet
            .Where(c => c.IdCours == idCours)
            .OrderBy(c => c.Libelle)
            .ToListAsync();
    }

    public async Task<Chapitre?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(c => c.CoursNav)
            .FirstOrDefaultAsync(c => c.IdChapitre == id && c.CoursNav.IdCompte == idCompte);
    }
}
