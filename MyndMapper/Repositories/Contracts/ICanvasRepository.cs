using MyndMapper.Entities;

namespace MyndMapper.Repositories.Contracts;

public interface ICanvasRepository : IGenericRepository<Canvas, int>
{
    public Task<Canvas?> GetWithUsersAsync(int key);

    public Task<bool> IsIdExist(int key);

    public Task RemoveAllAsync();
}