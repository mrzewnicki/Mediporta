using System.Text.Json.Serialization;

namespace Mediporta.StackOverflow.ApiClient.Models;

/// <summary>
/// Zewnętrzny link w collective
/// </summary>
public class ExternalLink
{
    /// <summary>
    /// np. "support", "learn", ...
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }
}