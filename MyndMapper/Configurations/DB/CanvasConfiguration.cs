using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyndMapper.Entities;

namespace MyndMapper.Configurations.DB;

public class CanvasConfiguration : IEntityTypeConfiguration<Canvas>
{
    public void Configure(EntityTypeBuilder<Canvas> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .IsRequired();

        builder.HasOne(e => e.Owner)
            .WithMany(e => e.CreatedCanvases)
            .IsRequired();
    }
}