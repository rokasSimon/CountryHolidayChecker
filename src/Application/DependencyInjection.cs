using System.Reflection;

using Application.Behaviours;
using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.Common.PreLoading;
using Application.CountryHolidays.CountryList.DTO;
using Application.CountryHolidays.DayStatus.DTO;
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

            config.AddRequestPreProcessor<GenericPreLoadingProcessor<GetCountryListRequest, CountryLoadData>>();
            config.AddRequestPreProcessor<GenericPreLoadingProcessor<GetGroupedHolidaysRequest, CountryHolidayLoadData>>();
            config.AddRequestPreProcessor<GenericPreLoadingProcessor<GetDayStatusRequest, CountryHolidayLoadData>>();
            config.AddRequestPreProcessor<GenericPreLoadingProcessor<GetMaximumFreeDaysRequest, CountryHolidayLoadData>>();

            //config.AddOpenRequestPreProcessor(typeof(GenericPreLoadingProcessor<,>));
        });

        services.AddScoped<IFreeDayCalculator, FreeDayCalculator>();

        return services;
    }
}
