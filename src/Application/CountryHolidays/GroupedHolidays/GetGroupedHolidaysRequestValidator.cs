using FluentValidation;

namespace Application.CountryHolidays.GroupedHolidays;

public class GetGroupedHolidaysRequestValidator : AbstractValidator<GetGroupedHolidaysRequest>
{
    public GetGroupedHolidaysRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleFor(p => p.Year)
            .InclusiveBetween(2011, 30000);
    }
}