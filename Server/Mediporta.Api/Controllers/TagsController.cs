using MediatR;
using Mediporta.Core.CQRS.Queries;
using Mediporta.Shared.DTOs;
using Mediporta.Shared.Querying;
using Microsoft.AspNetCore.Mvc;
using Mediporta.Api.Middleware;

namespace Mediporta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IQueryOptionsAccessor _queryOptionsAccessor;

    public TagsController(IMediator mediator, IQueryOptionsAccessor queryOptionsAccessor)
    {
        _mediator = mediator;
        _queryOptionsAccessor = queryOptionsAccessor;
    }

    /// <summary>
    /// Gets a paginated list of tags with optional sorting (supports multi-field: sort=Name:asc,Count:desc).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var request = new GetTagsQuery.Request(_queryOptionsAccessor.Options);
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}