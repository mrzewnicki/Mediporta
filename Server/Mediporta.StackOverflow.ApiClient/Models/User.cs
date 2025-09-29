using System.Text.Json.Serialization;

namespace Mediporta.StackOverflow.ApiClient.Models;

/// <summary>
/// Użytkownik w formie skróconej (często w wiki/last_editor)
/// </summary>
public class User
{
    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("profile_image")]
    public string? ProfileImage { get; set; }
}