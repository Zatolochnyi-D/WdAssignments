using MyndMapper.Entities;

namespace MyndMapper.Repositories.Contracts;

public interface IUserRepository : IGenericRepository<User, int>
{
    public Task<User?> GetWithCanvasesAsync(int key);

    public Task<bool> IsIdExist(int key);

    public Task RemoveAllAsync();
}