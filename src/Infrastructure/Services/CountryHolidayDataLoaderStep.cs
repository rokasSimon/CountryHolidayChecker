using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.External;
using Domain.Entities;

namespace Infrastructure.Services;

public class CountryHolidayDataLoaderStep(
    ICountryRepository _countryRepository,
    IHolidayRepository _holidayRepository,
    ICountryHolidayDataFetcher _holidayFetcher,
    ILoadStep<CountryLoadData> _countryLoadStep
    ) : ILoadStep<CountryHolidayLoadData>
{
    public async Task LoadAsync(CountryHolidayLoadData request)
    {
        if (await _countryLoadStep.ShouldLoadAsync(new CountryLoadData()))
        {
            await _countryLoadStep.LoadAsync(new CountryLoadData());
        }

        var country = await _countryRepository.GetCountryByCodeAsync(request.CountryCode)
            ?? throw new MissingResourceException($"Country:{request.CountryCode}");

        var fetchedHolidays = await _holidayFetcher.FetchHolidaysAsync(request.CountryCode, request.Year);
        var holidays = fetchedHolidays.Select(h =>
        {
            var holiday = new Holiday
            {
                Country = country,
                Names = h.Name.Select(n => new LocalizedName { Language = n.Lang, Text = n.Text }).ToArray(),
                Dates = [new HolidayDate { Date = new DateOnly(h.Date.Year, h.Date.Month, h.Date.Day) }]
            };

            return holiday;
        });

        await _holidayRepository.AddHolidaysToCountryAsync(holidays);
    }

    public async Task<bool> ShouldLoadAsync(CountryHolidayLoadData request)
    {
        var holidays = await _holidayRepository.GetAllHolidaysForCountryYearAsync(request.CountryCode, request.Year);

        return !holidays.Any();
    }
}
