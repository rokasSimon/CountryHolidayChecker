using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class CountryHolidayContext(DbContextOptions<CountryHolidayContext> options) : DbContext(options)
{
    public DbSet<Country> Countries{ get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<HolidayDate> HolidayDates { get; set; }
    public DbSet<LocalizedName> LocalizedNames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
