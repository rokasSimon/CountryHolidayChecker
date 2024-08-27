using Application.CountryHolidays.MaximumFreeDays.DTO;

using FluentValidation;

namespace Application.CountryHolidays.MaximumFreeDays.Validators;

public class GetMaximumFreeDaysRequestValidator : AbstractValidator<GetMaximumFreeDaysRequest>
{
    public GetMaximumFreeDaysRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleFor(p => p.Year)
            .InclusiveBetween(2011, 9999);
    }
}