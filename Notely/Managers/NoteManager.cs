using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class NoteManager : WriteableReadableManager<Note>
{
    public NoteManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Note>> GetAllByCompteAsync(int idCompte)
    {
        return await dbSet
            .Where(n => n.IdCompte == idCompte)
            .ToListAsync();
    }

    public async Task<Note?> GetByIdForCompteAsync(int id, int idCompte)
    {
        return await dbSet
            .FirstOrDefaultAsync(n => n.IdNote == id && n.IdCompte == idCompte);
    }
}
