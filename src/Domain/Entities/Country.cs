#nullable disable

namespace Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }

    public IEnumerable<Holiday> Holidays { get; set; }
}
