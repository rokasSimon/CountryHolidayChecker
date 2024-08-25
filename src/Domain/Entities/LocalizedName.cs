namespace Domain.Entities;

public class LocalizedName
{
    public int Id { get; set; }
    public required string Language { get; set; }
    public required string Text { get; set; }

    public Holiday Holiday { get; set; } = null!;
}
