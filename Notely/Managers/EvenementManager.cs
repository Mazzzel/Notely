using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class EvenementManager : WriteableReadableManager<Evenement>
{
    public EvenementManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Evenement>> GetAllByCompteAsync(int idCompte)
    {
        return await dbSet
            .Where(e => e.IdCompte == idCompte)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<Evenement?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .FirstOrDefaultAsync(e => e.IdEvenement == id && e.IdCompte == idCompte);
    }
}
