using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Repositories;

public class GenericRepository<TEntity, TKey>(DataModelContext context) : IGenericRepository<TEntity, TKey> where TEntity : class
{
    protected DataModelContext context = context;

    public virtual Task<TEntity?> GetAsync(TKey key)
    {
        return context.Set<TEntity>().FindAsync(key).AsTask();
    }

    public virtual IQueryable<TEntity> GetAllAsync()
    {
        return context.Set<TEntity>();
    }

    public virtual Task AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return context.SaveChangesAsync();
    }

    public virtual Task EditAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return context.SaveChangesAsync();
    }

    public virtual Task RemoveAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return context.SaveChangesAsync();
    }
}