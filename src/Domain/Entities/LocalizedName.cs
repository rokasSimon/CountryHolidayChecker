#nullable disable

namespace Domain.Entities;

public class LocalizedName
{
    public int Id { get; set; }
    public string Language { get; set; }
    public string Text { get; set; }

    public Holiday Holiday { get; set; }
}
