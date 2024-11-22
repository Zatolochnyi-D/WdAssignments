using MyndMapper.Entities;

namespace MyndMapper.Repositories.Contracts;

public interface ICanvasRepository : IGenericRepository<Canvas, int>
{
    public Task<Canvas?> GetWithUserAsync(int key);

    public Task RemoveAllAsync();
}