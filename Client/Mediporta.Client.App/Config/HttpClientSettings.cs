namespace Mediporta.Client.App.Config;

public class HttpClientSettings
{
    public string? BaseAddress { get; set; }

    public const string SectionName = "ApiHttpClient";
}