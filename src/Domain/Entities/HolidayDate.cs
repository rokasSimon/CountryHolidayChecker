namespace Domain.Entities;

public class HolidayDate
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }

    public int HolidayId { get; set; }
    public Holiday Holiday { get; set; } = null!;
}
