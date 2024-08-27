using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class HolidayRepository(CountryHolidayContext _context)
    : IHolidayRepository
{
    public async Task AddHolidaysToCountryAsync(IEnumerable<Holiday> holidays)
    {
        var missingHolidays = holidays.Where(x => !_context.Holidays.Any(z => z.Names.First() == x.Names.First())).ToList();

        _context.Holidays.AddRange(missingHolidays);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<HolidayDate>> GetAllHolidaysForCountryYearAsync(string countryCode, int year)
    {
        IEnumerable<HolidayDate> holidayDates = [];

        var country = await _context.Countries
            .Include(c => c.Holidays)
                .ThenInclude(h => h.Dates)
            .Include(c => c.Holidays)
                .ThenInclude(h => h.Names)
            .FirstOrDefaultAsync(c => c.CountryCode == countryCode);

        if (country != null)
        {
            holidayDates = country.Holidays
                .SelectMany(h => h.Dates)
                .Where(d => d.Date.Year == year);
        }

        return holidayDates;
    }

    public async Task<HolidayDate?> GetDateForCountryAsync(string countryCode, DateOnly date)
    {
        return await _context.HolidayDates
            .FirstOrDefaultAsync(d => d.Date == date && d.Holiday.Country.CountryCode == countryCode);
    }
}
