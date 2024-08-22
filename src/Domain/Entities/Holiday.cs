#nullable disable

namespace Domain.Entities;

public class Holiday
{
    public int Id { get; set; }
    public IEnumerable<HolidayDate> Dates { get; set; }
    public IEnumerable<LocalizedName> Names { get; set; }

    public Country Country { get; set; }
}