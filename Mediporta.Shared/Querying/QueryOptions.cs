using Mediporta.Api.Models;

namespace Mediporta.Shared.Querying;

public sealed record SortClause(string Field, SortDirection Direction);

public sealed class QueryOptions
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// Multiple sort clauses parsed from query like: sort=Name:asc,Count:desc
    /// </summary>
    public List<SortClause> Sorts { get; set; } = new();

    public void EnsureDefaults(
        int defaultPage = 1,
        int defaultPageSize = 50,
        string? defaultSortBy = null,
        SortDirection defaultDirection = SortDirection.Asc,
        IEnumerable<SortClause>? defaultSorts = null)
    {
        if (Page < 1)
            Page = defaultPage;

        if (PageSize < 1)
            PageSize = defaultPageSize;

        // If multi-sorts not provided, fall back to legacy single SortBy/Direction
        if (Sorts.Count == 0)
        {
            if (defaultSorts != null)
                Sorts.AddRange(defaultSorts);
            else if (!string.IsNullOrWhiteSpace(defaultSortBy))
                Sorts.Add(new SortClause(defaultSortBy!, defaultDirection));
        }
    }
}