using Application.CountryHolidays.DayStatus.DTO;
using FluentValidation;
namespace Application.CountryHolidays.DayStatus.Validators;

public class GetDayStatusRequestValidator : AbstractValidator<GetDayStatusRequest>
{
    public GetDayStatusRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleFor(p => p.Year)
            .InclusiveBetween(2011, 9999);
        RuleFor(p => p.Month)
            .InclusiveBetween(1, 12);
        RuleFor(p => p.Day)
            .InclusiveBetween(1, 31);
    }
}
