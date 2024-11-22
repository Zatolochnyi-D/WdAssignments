namespace MyndMapper.Repositories.Contracts;

public interface IGenericRepository<TEntity, TKey> where TEntity : class
{
    public Task<TEntity?> GetAsync(TKey key);

    public IQueryable<TEntity> GetAllAsync();

    public Task AddAsync(TEntity entity);

    public Task EditAsync(TEntity entity);

    public Task RemoveAsync(TEntity entity);
}