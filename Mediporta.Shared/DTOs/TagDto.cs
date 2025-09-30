namespace Mediporta.Shared.DTOs;

public sealed record TagDto(int Id, string Name, int Count, bool HasSynonyms, bool IsModeratorOnly, bool IsRequired, double PercentageOfAll);

public sealed record GetTagResultDto(IEnumerable<TagDto> Tags, int TotalCount);