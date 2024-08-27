namespace Infrastructure.Common.Exceptions;

public class MissingResourceException(string missingResource)
    : Exception($"The following resource could not be found '{missingResource}'.")
{
    public string MissingResource { get; init; } = missingResource;
}
