using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Repositories;

public class CanvasRepository(DataModelContext context) : GenericRepository<Canvas, int>(context), ICanvasRepository
{
    public Task<Canvas?> GetWithUserAsync(int key)
    {
        return context.Set<Canvas>().Include(e => e.Owner).FirstOrDefaultAsync(x => x.Id == key);
    }

    public Task RemoveAllAsync()
    {
        return context.Set<Canvas>().ExecuteDeleteAsync();
    }
}