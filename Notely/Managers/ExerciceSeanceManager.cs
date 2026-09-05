using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class ExerciceSeanceManager : WriteableReadableManager<ExerciceSeance>
{
    public ExerciceSeanceManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<ExerciceSeance?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(e => e.SeanceNav)
            .Include(e => e.Series)
            .FirstOrDefaultAsync(e => e.IdExerciceSeance == id && e.SeanceNav.IdCompte == idCompte);
    }
}
