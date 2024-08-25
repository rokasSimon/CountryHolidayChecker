using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .HasIndex(c => c.CountryCode)
            .IsUnique();

        builder.Property(c => c.CountryCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(c => c.CountryName)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .HasMany(c => c.Holidays)
            .WithOne(h => h.Country)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
