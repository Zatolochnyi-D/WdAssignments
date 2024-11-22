using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyndMapper.Entities;

namespace MyndMapper.Configurations.DB;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.Email)
            .IsRequired();

        builder.Property(e => e.Password)
            .IsRequired();

        builder.HasMany(e => e.CreatedCanvases)
            .WithOne(e => e.Owner)
            .IsRequired();
    }
}