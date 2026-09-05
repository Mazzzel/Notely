using Microsoft.EntityFrameworkCore;
using Notely.Data;
using Notely.Entities;

namespace Notely.Managers;

public class CompteManager : WriteableReadableManager<Compte>
{
    public CompteManager(NotelyDbContext context) : base(context)
    {
    }

    public async Task<Compte?> GetByEmailAsync(string email)
    {
        return await dbSet.FirstOrDefaultAsync(c => c.Email == email);
    }
}
