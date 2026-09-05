using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Dto;
using Notely.Entities;

namespace Notely.Managers;

public class SeanceManager : WriteableReadableManager<Seance>
{
    public SeanceManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Seance>> GetAllByCompteAsync(int idCompte)
    {
        return await dbSet
            .Include(s => s.ExercicesSeance)
            .ThenInclude(e => e.Series)
            .Where(s => s.IdCompte == idCompte)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<Seance?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .Include(s => s.ExercicesSeance)
            .ThenInclude(e => e.Series)
            .FirstOrDefaultAsync(s => s.IdSeance == id && s.IdCompte == idCompte);
    }

    public async Task<IEnumerable<string>> GetNomsExercicesDistinctsAsync(int idCompte)
    {
        return await context.Set<ExerciceSeance>()
            .Where(e => e.SeanceNav.IdCompte == idCompte)
            .Select(e => e.Nom)
            .Distinct()
            .OrderBy(n => n)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProgressionPointDTO>> GetProgressionAsync(int idCompte, string nomExercice)
    {
        return await context.Set<Serie>()
            .Where(s => s.ExerciceSeanceNav.SeanceNav.IdCompte == idCompte
                        && s.ExerciceSeanceNav.Nom.ToLower() == nomExercice.ToLower())
            .GroupBy(s => s.ExerciceSeanceNav.SeanceNav.Date)
            .Select(g => new ProgressionPointDTO { Date = g.Key, PoidsMax = g.Max(s => s.Poids) })
            .OrderBy(p => p.Date)
            .ToListAsync();
    }
}
