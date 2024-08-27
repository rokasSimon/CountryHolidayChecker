using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CountryRepository(CountryHolidayContext _context) : ICountryRepository
{
    public async Task AddCountriesAsync(IEnumerable<Country> countries)
    {
        var missingCountries = countries.Where(x => !_context.Countries.Any(z => z.CountryCode == x.CountryCode)).ToList();

        _context.Countries.AddRange(missingCountries);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByCodeAsync(string countryCode)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.CountryCode == countryCode);
    }

    public async Task<bool> IsEmptyAsync()
    {
        var tableContainsCountries = await _context.Countries.AnyAsync();

        return !tableContainsCountries;
    }
}
