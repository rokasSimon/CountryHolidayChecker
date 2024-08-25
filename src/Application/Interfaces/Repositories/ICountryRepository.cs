using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICountryRepository
{
    Task<bool> IsEmptyAsync();
    Task<Country?> GetCountryByCodeAsync(string countryCode);
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task AddCountriesAsync(IEnumerable<Country> countries);
}
