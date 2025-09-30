using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediporta.Core.CQRS.Queries;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories;
using Mediporta.Data.Repositories.Helpers;
using Mediporta.Shared.Querying;
using Moq;
using Xunit;

namespace Mediporta.Tests.Unit;

public class GetTagsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Map_And_Sort_By_Name_Asc()
    {
        var data = new List<Tag>
        {
            new Tag { Id = 1, Name = "csharp", PercentageOfAll = 10 },
            new Tag { Id = 2, Name = "aspnet", PercentageOfAll = 20 },
        };

        var repo = new Mock<ITagsRepository>();
        repo.Setup(r => r.GetAll(It.IsAny<IReadOnlyList<OrderByQuery>>(), 0, 50))
            .Returns(data.OrderBy(t => t.Name));
        repo.Setup(r => r.CountAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(2);

        var handler = new GetTagsQuery.Handler(repo.Object);
        var options = QueryOptions.Build("Name:asc", page: 1, pageSize: 50);
        var response = await handler.Handle(new GetTagsQuery.Request(options), CancellationToken.None);

        response.Items.Select(i => i.Name).Should().ContainInOrder("aspnet", "csharp");
        response.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_Should_Compute_Skip_Correctly_For_Page2()
    {
        var repo = new Mock<ITagsRepository>(MockBehavior.Strict);
        repo.Setup(r => r.GetAll(It.IsAny<IReadOnlyList<OrderByQuery>>(), 10, 10))
            .Returns(new List<Tag>())
            .Verifiable();
        repo.Setup(r => r.CountAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        var handler = new GetTagsQuery.Handler(repo.Object);
        var options = QueryOptions.Build("Name:asc", page: 2, pageSize: 10);

        // NOTE: Current implementation may fail this verification due to skip miscalculation
        await handler.Handle(new GetTagsQuery.Request(options), CancellationToken.None);
        repo.Verify();
    }
}