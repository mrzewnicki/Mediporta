using Mediporta.StackOverflow.ApiClient.Models;
using Mediporta.StackOverflow.ApiClient.Models.Api;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;
using System.Text.Json;

namespace Mediporta.StackOverflow.ApiClient;

public class StackOverflowApiClient:IStackOverflowApiClient
{
    private readonly HttpClient _httpClient;
    private readonly StackOverflowApiClientConfiguration _configuration;

    public StackOverflowApiClient(HttpClient httpClient, StackOverflowApiClientConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri("https://api.stackexchange.com/2.3/");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_configuration.RegisteredAppUserAgent);
    }

    private string BuildGetTagsEndpointUrl(GetTags.Request request, int page, int pageSize)
    {
        var order = request.Order switch
        {
            OrderDirection.Asc => "asc",
            OrderDirection.Desc => "desc",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"tags?page={page}&pagesize={pageSize}&order={order}&sort={request.Sort}&site={request.Site}&key={_configuration.ApiKey}";
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync(GetTags.Request request)
    {
        const int pageSize = 100;
        var tags = new List<Tag>();
        var currentPage = 1;
        var hasMore = true;

        while (tags.Count < request.ItemsCountToFetch && hasMore)
        {
            var url = BuildGetTagsEndpointUrl(request, currentPage, pageSize);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tagResponse = JsonSerializer.Deserialize<GetTags.Response>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tagResponse?.Items != null)
                tags.AddRange(tagResponse.Items);

            hasMore = tagResponse?.HasMore ?? false;
            currentPage++;
        }
        return tags.Count > request.ItemsCountToFetch
            ? tags.GetRange(0, request.ItemsCountToFetch)
            : tags;
    }
}
