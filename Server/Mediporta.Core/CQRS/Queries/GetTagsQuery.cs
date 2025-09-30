using MediatR;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories;
using Mediporta.Data.Repositories.Helpers;
using Mediporta.Shared.DTOs;
using Mediporta.Shared.Querying;

namespace Mediporta.Core.CQRS.Queries;

public static class GetTagsQuery
{
    public sealed record Request(QueryOptions Options) : IRequest<Response>;

    public sealed record Response(IEnumerable<TagDto> Items, int TotalCount);


    public sealed class Handler(ITagsRepository repository) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var orderByQueries = request.Options.Sorts.Select(CreateOrderByQuery);
            var skip = request.Options.Page <= 1 ? 0
                    : (request.Options.Page - 1) * request.Options.PageSize;
            var take = request.Options.PageSize;

            var tags = repository.GetAll(orderByQueries.ToList(), skip, take);
            var items = tags
                .Select(t => new TagDto(t.Id, t.Name, t.Count, t.HasSynonyms, t.IsModeratorOnly, t.IsRequired, t.PercentageOfAll))
                .AsEnumerable();

            var countAll = await repository.CountAllAsync(cancellationToken);

            return new Response(items, countAll);
        }

        private static OrderByQuery CreateOrderByQuery(SortClause clause)
        {
            // Add more field to let order by'em
            Func<Tag, object> orderQuery = clause.Field switch
            {
                nameof(Tag.Name) => tag => tag.Name,
                nameof(Tag.PercentageOfAll) => tag => tag.PercentageOfAll,
                _ => t => t.Name
            };

            var isAscending = clause.Direction == SortDirection.Asc;
            return new OrderByQuery(orderQuery, isAscending);
        }
    }
}