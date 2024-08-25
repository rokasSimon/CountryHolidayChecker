namespace Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public required string CountryCode { get; set; }
    public required string CountryName { get; set; }

    public ICollection<Holiday> Holidays { get; set; } = [];
}
