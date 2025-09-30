using MediatR;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories;
using Mediporta.StackOverflow.ApiClient;
using Mediporta.StackOverflow.ApiClient.Models.Api;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;

namespace Mediporta.Core.CQRS.Command;

public static class DownloadTagsFromStackOverflow
{
    public sealed record Request(int ItemsCount, OrderDirection OrderDirection, string Sort, string Site) : IRequest<Response>;

    public sealed record Response();


    public sealed class Handler(IStackOverflowApiClient stackOverflowApiClient, ITagsRepository tagsRepository) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var stackoverflowRequest = new GetTags.Request()
            {
                ItemsCountToFetch = request.ItemsCount,
                Order = request.OrderDirection,
                Sort = request.Sort,
                Site = request.Site
            };

            var fetchedTags = await stackOverflowApiClient.GetTagsAsync(stackoverflowRequest);

            var sumOfAll = fetchedTags.Sum(t => t.Count.HasValue ? t.Count.Value : 0);

            var s = fetchedTags.Where(x => x.HasSynonyms == true || x.IsModeratorOnly == true || x.IsRequired == true).ToList();

            var tagsToAdd = fetchedTags.Select(t => new Tag
            {
                Name = t.Name,
                Count = t.Count.HasValue ? t.Count.Value : 0,
                HasSynonyms = t.HasSynonyms.HasValue ? t.HasSynonyms.Value : false,
                IsModeratorOnly = t.IsModeratorOnly.HasValue ? t.IsModeratorOnly.Value : false,
                IsRequired = t.IsRequired.HasValue ? t.IsRequired.Value : false,
                PercentageOfAll = t.Count.HasValue ? (double)t.Count.Value / sumOfAll * 100 : 0
            }).ToList();

            await tagsRepository.AddRangeAsync(tagsToAdd);

            return new Response();
        }
    }
}