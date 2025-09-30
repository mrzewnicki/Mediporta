using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediporta.Client.App.Services;
using Mediporta.Shared.DTOs;
using Moq;
using Moq.Protected;
using MudBlazor;
using Xunit;

namespace Mediporta.Client.Tests.Unit;

public class TagsServiceTests
{
    private static HttpClient CreateHttpClient(Mock<HttpMessageHandler> handlerMock)
    {
        return new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost/api/") };
    }

    [Fact]
    public async Task GetTags_Should_Call_Correct_Endpoint_And_Return_Data()
    {
        var responseDto = new GetTagResultDto(new[] { new TagDto(1, "csharp", 1, false, false, false, 1.0) }, 1);

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken _) =>
            {
                req.RequestUri!.ToString().Should().Contain("Tags/GetAll?sort=Name:asc&page=2&pageSize=25");
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(responseDto)
                };
            });

        var snackbar = Mock.Of<ISnackbar>();
        var httpClient = CreateHttpClient(handler);
        var service = new TagsService(new FakeHttpClientFactory(httpClient), snackbar);

        var result = await service.GetTags($"{nameof(TagDto.Name)}:asc", 2, 25);
        result.TotalCount.Should().Be(1);
        result.Tags.Should().HaveCount(1);
    }

    [Fact]
    public async Task RefreshData_Should_Post_Correct_Body_And_Handle_Errors()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken _) =>
            {
                req.Method.Should().Be(HttpMethod.Post);
                req.RequestUri!.ToString().Should().Contain("Tags/RefreshData");
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            });

        var service = new TagsService(new FakeHttpClientFactory(CreateHttpClient(handler)), Mock.Of<ISnackbar>());
        await service.RefreshData();
    }

    private class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;
        public FakeHttpClientFactory(HttpClient client) => _client = client;
        public HttpClient CreateClient(string name) => _client;
    }
}