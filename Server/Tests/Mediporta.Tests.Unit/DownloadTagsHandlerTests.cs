using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediporta.Core.CQRS.Command;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories;
using Mediporta.StackOverflow.ApiClient;
using Mediporta.StackOverflow.ApiClient.Models.Api;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;
using Moq;
using Xunit;

namespace Mediporta.Tests.Unit;

public class DownloadTagsHandlerTests
{
    [Fact]
    public async Task Handle_Should_Map_Request_Call_Api_And_Save_Tags_With_Percentage()
    {
        var api = new Mock<IStackOverflowApiClient>();
        api.Setup(a => a.GetTagsAsync(It.IsAny<GetTags.Request>()))
           .ReturnsAsync(new[]
           {
               new Mediporta.StackOverflow.ApiClient.Models.Tag { Name = "csharp", Count = 100 },
               new Mediporta.StackOverflow.ApiClient.Models.Tag { Name = "aspnet", Count = 300 },
           });

        var repo = new Mock<ITagsRepository>();
        repo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Tag>>()))
            .Callback<IEnumerable<Tag>>(tags =>
            {
                var list = tags.ToList();
                list.Should().HaveCount(2);
                list.First(t => t.Name == "csharp").PercentageOfAll.Should().BeApproximately(25, 0.001);
                list.First(t => t.Name == "aspnet").PercentageOfAll.Should().BeApproximately(75, 0.001);
            })
            .Returns(Task.CompletedTask);

        var handler = new DownloadTagsFromStackOverflow.Handler(api.Object, repo.Object);
        await handler.Handle(new DownloadTagsFromStackOverflow.Request(1000, OrderDirection.Asc, "popular", "stackoverflow"), CancellationToken.None);

        api.Verify(a => a.GetTagsAsync(It.Is<GetTags.Request>(r => r.ItemsCountToFetch == 1000 && r.Order == OrderDirection.Asc && r.Sort == "popular" && r.Site == "stackoverflow")), Times.Once);
        repo.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<Tag>>()), Times.Once);
    }
}