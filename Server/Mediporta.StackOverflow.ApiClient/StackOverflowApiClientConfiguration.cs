namespace Mediporta.StackOverflow.ApiClient;

public class StackOverflowApiClientConfiguration
{
    public required string ApiKey { get; set; }
    public required string? RegisteredAppUserAgent { get; set; }
}