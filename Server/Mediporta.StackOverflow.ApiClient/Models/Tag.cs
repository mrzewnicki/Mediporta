namespace Mediporta.StackOverflow.ApiClient.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;


/// <summary>
/// Model pojedynczego taga
/// </summary>
public class Tag
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("has_synonyms")]
    public bool? HasSynonyms { get; set; }

    [JsonPropertyName("is_moderator_only")]
    public bool? IsModeratorOnly { get; set; }

    [JsonPropertyName("is_required")]
    public bool? IsRequired { get; set; }

    [JsonPropertyName("collectives")]
    public List<Collective>? Collectives { get; set; }

    // Opcjonalne pola, które mogą pojawić się w szerszych odpowiedziach o tagach
    [JsonPropertyName("synonyms")]
    public List<string>? Synonyms { get; set; }

    [JsonPropertyName("excerpt")]
    public string? Excerpt { get; set; }

    [JsonPropertyName("wiki")]
    public TagWiki? Wiki { get; set; }
}