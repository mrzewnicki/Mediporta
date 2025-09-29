using Mediporta.StackOverflow.ApiClient.Models;
using Mediporta.StackOverflow.ApiClient.Models.Api;

namespace Mediporta.StackOverflow.ApiClient;

public interface IStackOverflowApiClient
{
    Task<IEnumerable<Tag>> GetTagsAsync(GetTags.Request request);
}