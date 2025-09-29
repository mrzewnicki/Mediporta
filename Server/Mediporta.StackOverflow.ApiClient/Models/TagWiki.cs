using System.Text.Json.Serialization;

namespace Mediporta.StackOverflow.ApiClient.Models;

/// <summary>
/// Opcjonalna encja wiki/excerpt dla taga (pewne endpointy zwracają to)
/// </summary>
public class TagWiki
{
    [JsonPropertyName("excerpt")]
    public string? Excerpt { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    /// <summary>
    /// Najczęściej pole daty podawane jest jako unix epoch (sekundy).
    /// </summary>
    [JsonPropertyName("last_edit_date")]
    public long? LastEditDateUnix { get; set; }

    [JsonPropertyName("last_editor")]
    public User? LastEditor { get; set; }

    [JsonIgnore]
    public DateTimeOffset? LastEditDate => LastEditDateUnix.HasValue
        ? DateTimeOffset.FromUnixTimeSeconds(LastEditDateUnix.Value)
        : null;
}