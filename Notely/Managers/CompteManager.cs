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
        return await dbSet.Include(c => c.AccesPages).FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Compte?> GetByIdWithAccesPagesAsync(int id)
    {
        return await dbSet.Include(c => c.AccesPages).FirstOrDefaultAsync(c => c.IdCompte == id);
    }

    public async Task<List<Compte>> GetAllWithAccesPagesAsync()
    {
        return await dbSet.Include(c => c.AccesPages).OrderBy(c => c.Email).ToListAsync();
    }
}
