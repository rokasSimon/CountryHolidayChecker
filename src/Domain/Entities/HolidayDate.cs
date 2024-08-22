#nullable disable

namespace Domain.Entities;

public class HolidayDate
{
    public int Id { get; set; }
    // If year is null, then holiday is not a variable date
    public int? Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int DayOfWeek { get; set; }

    public Holiday Holiday { get; set; }
}
