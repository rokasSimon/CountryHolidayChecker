using System.Reflection;

using Application.Behaviours;
using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.CountryList.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.CountryHolidays.DayStatus.Utility;
using Application.CountryHolidays.GroupedHolidays.DTO;
using Application.CountryHolidays.MaximumFreeDays.DTO;
using Application.CountryHolidays.MaximumFreeDays.Utility;
using Application.Interfaces.Services.Utility;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            config.AddBehavior<PreLoadingBehaviour<GetCountryListRequest, CountryLoadData, GetCountryListResult>>();
            config.AddBehavior<PreLoadingBehaviour<GetGroupedHolidaysRequest, CountryHolidayLoadData, GetGroupedHolidaysResult>>();
            config.AddBehavior<PreLoadingBehaviour<GetDayStatusRequest, CountryHolidayLoadData, GetDayStatusResult>>();
            config.AddBehavior<PreLoadingBehaviour<GetMaximumFreeDaysRequest, CountryHolidayLoadData, GetMaximumFreeDaysResult>>();
        });

        services.AddScoped<IFreeDayCalculator, FreeDayCalculator>();
        services.AddScoped<IDayTypeChecker, DayTypeChecker>();

        return services;
    }
}
