using Domain.Entities;

namespace Application.Interfaces.Services.Utility;

public interface IFreeDayCalculator
{
    (int days, DateOnly startDate, DateOnly endDate) FindMaximumRowOfFreeDays(int year, IReadOnlySet<DateOnly> holidayDates);
}
