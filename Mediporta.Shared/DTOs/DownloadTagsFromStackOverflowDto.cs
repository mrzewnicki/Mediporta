namespace Mediporta.Shared.DTOs;

public sealed class DownloadTagsFromStackOverflowDto
{
    public int ItemsCount { get; set; }

    public string OrderDirection { get; set; } = "desc";

    public string Sort { get; set; } = "popular";

    public string Site { get; set; } = "stackoverflow";
}
