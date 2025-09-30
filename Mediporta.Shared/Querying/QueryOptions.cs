using Mediporta.Shared.Helpers;

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

    public static QueryOptions Build(string sortExpression, int page, int pageSize)
    {
        if(page < 1)
            page = 1;

        if(pageSize < 1)
            pageSize = DefaultPaginationHelper.PageSize;

        var options = new QueryOptions()
        {
            Page = page,
            PageSize = pageSize
        };

        if (!string.IsNullOrEmpty(sortExpression))
        {
            if (sortExpression.Contains(','))
                foreach (var item in sortExpression.Split(','))
                    options.Sorts.AddRange(StringToSortClause(item));
            else
                options.Sorts.AddRange(StringToSortClause(sortExpression));
        }

        return options;
    }

    private static SortClause StringToSortClause(string s)
    {
        var splittedArguments = s.Split(":");
        var fieldString = splittedArguments[0];
        var directionString = splittedArguments[1];


        return new SortClause(fieldString, StringToSortDirection(directionString));
    }

    private static SortDirection StringToSortDirection(string s) => s switch
    {
        "asc" => SortDirection.Asc,
        "desc" => SortDirection.Desc,
        _ => SortDirection.Asc
    };
}