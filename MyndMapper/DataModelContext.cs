using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;

namespace MyndMapper;

public class DataModelContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Canvas> Canvases { get; set; }

    public DataModelContext(DbContextOptions<DataModelContext> options) : base(options) {} 
}