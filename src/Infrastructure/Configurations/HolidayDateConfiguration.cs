using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class HolidayDateConfiguration : IEntityTypeConfiguration<HolidayDate>
{
    public void Configure(EntityTypeBuilder<HolidayDate> builder)
    {
        builder
            .HasKey(h => h.Id);

        builder
            .HasIndex(h => new { h.Date, h.HolidayId });
    }
}
