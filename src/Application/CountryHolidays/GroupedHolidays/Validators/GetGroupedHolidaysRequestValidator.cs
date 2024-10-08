﻿using Application.CountryHolidays.GroupedHolidays.DTO;

using FluentValidation;

namespace Application.CountryHolidays.GroupedHolidays.Validators;

public class GetGroupedHolidaysRequestValidator : AbstractValidator<GetGroupedHolidaysRequest>
{
    public GetGroupedHolidaysRequestValidator()
    {
        RuleFor(p => p.CountryCode)
            .Length(3);
        RuleFor(p => p.Year)
            .InclusiveBetween(2011, 9999);
    }
}