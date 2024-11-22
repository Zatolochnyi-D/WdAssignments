using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Repositories;

public class GenericRepository<TEntity, TKey>(DataModelContext context) : IGenericRepository<TEntity, TKey> where TEntity : class
{
    public virtual Task<TEntity?> GetAsync(TKey key)
    {
        return context.Set<TEntity>().FindAsync(key).AsTask();
    }

    public IQueryable<TEntity> GetAllAsync()
    {
        return context.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return context.SaveChangesAsync();
    }

    public Task EditAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return context.SaveChangesAsync();
    }

    public Task RemoveAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return context.SaveChangesAsync();
    }
}