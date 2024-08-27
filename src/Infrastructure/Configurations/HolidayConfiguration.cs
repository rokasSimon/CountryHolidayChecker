using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder
            .HasKey(h => h.Id);

        builder
            .HasMany(h => h.Names)
            .WithOne(n => n.Holiday)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(h => h.Dates)
            .WithOne(d => d.Holiday)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
