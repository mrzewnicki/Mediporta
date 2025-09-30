using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Mediporta.Api.Controllers;
using Mediporta.Core.CQRS.Command;
using Mediporta.Core.CQRS.Queries;
using Mediporta.Shared.DTOs;
using Mediporta.Shared.Querying;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Mediporta.Tests.Unit;

public class TagsControllerTests
{
    [Fact]
    public async Task GetAll_Should_Return_Ok_With_Data()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetTagsQuery.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetTagsQuery.Response(new[] { new TagDto(1, "csharp", 1, false, false, false, 1.0) }, 1));

        var controller = new TagsController(mediator.Object);
        var result = await controller.GetAll("Name:asc", 1, 10, CancellationToken.None) as OkObjectResult;

        result.Should().NotBeNull();
        var dto = result!.Value as GetTagResultDto;
        dto!.TotalCount.Should().Be(1);
        dto!.Tags.Should().HaveCount(1);
    }
}