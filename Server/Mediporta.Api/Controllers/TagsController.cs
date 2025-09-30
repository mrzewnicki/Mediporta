using MediatR;
using Mediporta.Core.CQRS.Queries;
using Mediporta.Core.CQRS.Command;
using Mediporta.Shared.DTOs;
using Mediporta.Shared.Helpers;
using Mediporta.Shared.Querying;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Mediporta.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of tags with optional sorting (supports multi-field: sort=Name:asc,Count:desc).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(string sort = "Name:asc", int page = 1, int pageSize = DefaultPaginationHelper.PageSize, CancellationToken cancellationToken = default)
    {
        var queryOptions = QueryOptions.Build(sort, page, pageSize);

        if(queryOptions is null)
            return BadRequest("Invalid query parametrs");

        var request = new GetTagsQuery.Request(queryOptions);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(new GetTagResultDto(response.Items, response.TotalCount));
    }

    /// <summary>
    /// Downloads tags from StackOverflow and stores them in the database.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshData([FromBody] DownloadTagsFromStackOverflowDto dto, CancellationToken cancellationToken = default)
    {
        var removeDataCommand = new DeleteAllTags.Request();
        var removeResult = await _mediator.Send(removeDataCommand, cancellationToken);

        var order = dto.OrderDirection.ToLower() == "asc"
            ? OrderDirection.Asc
            : OrderDirection.Desc;

        var request = new DownloadTagsFromStackOverflow.Request(
            dto.ItemsCount,
            order,
            dto.Sort,
            dto.Site
        );

        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }
}