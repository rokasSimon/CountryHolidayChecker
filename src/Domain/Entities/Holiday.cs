namespace Domain.Entities;

public class Holiday
{
    public int Id { get; set; }
    public ICollection<HolidayDate> Dates { get; set; } = [];
    public ICollection<LocalizedName> Names { get; set; } = [];

    public Country Country { get; set; } = null!;
}