namespace Notely.Managers.Interfaces;

public interface WritableRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entityToUpdate, T entity);
    Task DeleteAsync(T entity);
}
