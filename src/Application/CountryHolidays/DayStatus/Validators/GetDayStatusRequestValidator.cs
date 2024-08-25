using Application.CountryHolidays.DayStatus.DTO;

using FluentValidation;

namespace Application.CountryHolidays.DayStatus.Validators;

public class GetDayStatusRequestValidator : AbstractValidator<GetDayStatusRequest>
{
    public GetDayStatusRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleSet("ValidDate", () => RuleFor(p => p.Date.Year).InclusiveBetween(2011, 9999));
    }
}
