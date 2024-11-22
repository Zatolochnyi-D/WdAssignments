using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyndMapper.Entities;

namespace MyndMapper;

public class DataModelContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Canvas> Canvases { get; set; } = null!;

    public DataModelContext(DbContextOptions<DataModelContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}