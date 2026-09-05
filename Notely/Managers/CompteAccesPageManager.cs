using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class CompteAccesPageManager : WriteableReadableManager<CompteAccesPage>
{
    public CompteAccesPageManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<bool> HasAccessAsync(int idCompte, string codePage)
    {
        return await dbSet.AnyAsync(a => a.IdCompte == idCompte && a.CodePage == codePage);
    }

    public async Task SetForCompteAsync(int idCompte, IEnumerable<string> codesPage)
    {
        var existants = await dbSet.Where(a => a.IdCompte == idCompte).ToListAsync();
        dbSet.RemoveRange(existants);

        foreach (var code in codesPage)
            await dbSet.AddAsync(new CompteAccesPage { IdCompte = idCompte, CodePage = code });

        await context.SaveChangesAsync();
    }
}
