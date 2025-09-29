using System.Text.Json.Serialization;

namespace Mediporta.StackOverflow.ApiClient.Models;

/// <summary>
/// Collective powiązane z tagiem
/// </summary>
public class Collective
{
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    [JsonPropertyName("external_links")]
    public List<ExternalLink>? ExternalLinks { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("logo")]
    public ImageInfo? Logo { get; set; }
}