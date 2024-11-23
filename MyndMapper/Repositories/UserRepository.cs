using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Repositories;

public class UserRepository(DataModelContext context) : GenericRepository<User, int>(context), IUserRepository
{
    public override IQueryable<User> GetAllAsync()
    {
        return context.Set<User>().Include(e => e.CreatedCanvases).AsNoTracking();
    }

    public Task<User?> GetWithCanvasesAsync(int key)
    {
        return context.Set<User>().Include(e => e.CreatedCanvases).FirstOrDefaultAsync(x => x.Id == key);
    }

    public Task<bool> IsIdExist(int key)
    {
        return context.Set<User>().AsNoTracking().Where(x => x.Id == key).AnyAsync();
    }

    public Task RemoveAllAsync()
    {
        return context.Set<User>().ExecuteDeleteAsync();
    }
}