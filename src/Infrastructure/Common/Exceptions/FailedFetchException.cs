namespace Infrastructure.Common.Exceptions;

public class FailedFetchException(string fetchUrl)
    : Exception($"Could not receive data from endpoint '{fetchUrl}'")
{
    public string FetchUrl { get; init; } = fetchUrl;
}
