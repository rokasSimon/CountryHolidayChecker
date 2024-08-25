using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class LocalizedNameConfiguration : IEntityTypeConfiguration<LocalizedName>
{
    public void Configure(EntityTypeBuilder<LocalizedName> builder)
    {
        builder
            .HasKey(n => n.Id);

        builder
            .Property(n => n.Language)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(n => n.Text)
            .HasMaxLength(60)
            .IsRequired();
    }
}
