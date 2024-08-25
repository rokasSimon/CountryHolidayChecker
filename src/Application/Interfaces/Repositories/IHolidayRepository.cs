using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IHolidayRepository
{
    Task<HolidayDate?> GetDateForCountryAsync(string countryCode, DateOnly date);
    Task<IEnumerable<HolidayDate>> GetAllHolidaysForCountryYearAsync(string countryCode, int year);

    Task AddHolidaysToCountryAsync(IEnumerable<Holiday> holidays);
}
