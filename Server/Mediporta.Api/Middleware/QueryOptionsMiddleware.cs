using Mediporta.Api.Models;
using Mediporta.Shared.Querying;

namespace Mediporta.Api.Middleware;

public interface IQueryOptionsAccessor
{
    QueryOptions Options { get; set; }
}

public sealed class QueryOptionsAccessor : IQueryOptionsAccessor
{
    public QueryOptions Options { get; set; } = new();
}

public class QueryOptionsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IQueryOptionsAccessor accessor)
    {
        var query = context.Request.Query;
        var options = new QueryOptions();

        if (query.TryGetValue("page", out var pageVals) && int.TryParse(pageVals.ToString(), out var page))
            options.Page = page;

        if (query.TryGetValue("pageSize", out var pageSizeVals) && int.TryParse(pageSizeVals.ToString(), out var pageSize))
            options.PageSize = pageSize;

        if (query.TryGetValue("sort", out var sortVals))
        {
            var sort = sortVals.ToString();

            if (sort.Contains(','))
                foreach (var item in sort.Split(','))
                    options.Sorts.AddRange(StringToSortClause(item));
            else
                options.Sorts.AddRange(StringToSortClause(sort));
        }

        options.EnsureDefaults();

        accessor.Options = options;

        await next(context);
    }

    private IEnumerable<SortClause> StringToSortClause(string s) => s.Split(":")
        .ToDictionary(p => p[0].ToString(), p => StringToSortDirection(p[1].ToString()))
        .Select(pair => new SortClause(pair.Key, pair.Value));

    private static SortDirection StringToSortDirection(string s) => s switch
    {
        "asc" => SortDirection.Asc,
        "desc" => SortDirection.Desc,
        _ => SortDirection.Asc
    };
}