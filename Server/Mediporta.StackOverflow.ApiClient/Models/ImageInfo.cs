using System.Text.Json.Serialization;

namespace Mediporta.StackOverflow.ApiClient.Models;

public class ImageInfo
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }
}