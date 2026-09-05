using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class SerieManager : WriteableReadableManager<Serie>
{
    public SerieManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<Serie?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(s => s.ExerciceSeanceNav)
            .ThenInclude(e => e.SeanceNav)
            .FirstOrDefaultAsync(s => s.IdSerie == id && s.ExerciceSeanceNav.SeanceNav.IdCompte == idCompte);
    }
}
