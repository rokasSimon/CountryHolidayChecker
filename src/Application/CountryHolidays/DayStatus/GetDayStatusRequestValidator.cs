using FluentValidation;

namespace Application.CountryHolidays.DayStatus;

public class GetDayStatusRequestValidator : AbstractValidator<GetDayStatusRequest>
{
    public GetDayStatusRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleSet("ValidDate", () =>
        {
            RuleFor(p => p.Year).InclusiveBetween(2011, 30000);
            RuleFor(p => p.Month).InclusiveBetween(1, 12);
            RuleFor(p => p.Day).InclusiveBetween(1, 31);
        });
    }
}
