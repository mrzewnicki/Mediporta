using System.Text.Json.Serialization;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;

namespace Mediporta.StackOverflow.ApiClient.Models.Api;

public class GetTags
{
    public class Request
    {
        public required int ItemsCountToFetch { get; set; }
        public OrderDirection Order { get; set; } = OrderDirection.Desc;
        public string Sort { get; set; } = "popular";
        public string Site { get; set; } = "stackoverflow";
    }

    public class Response
    {
        [JsonPropertyName("items")]
        public IEnumerable<Tag> Items { get; set; } = [];

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("quota_max")]
        public int? QuotaMax { get; set; }

        [JsonPropertyName("quota_remaining")]
        public int? QuotaRemaining { get; set; }

        /// <summary>
        /// Czas (w sekundach), którego klient powinien przestrzegać przed kolejnym requestem (opcjonalne)
        /// </summary>
        [JsonPropertyName("backoff")]
        public int? Backoff { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
    }
}