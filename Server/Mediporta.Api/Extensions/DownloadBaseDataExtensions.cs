using MediatR;
using Mediporta.Core.CQRS.Command;
using Mediporta.StackOverflow.ApiClient.Models.Helpers;

namespace Mediporta.Api.Extensions;

public static class DownloadBaseDataExtensions
{
    public static IServiceProvider DownloadBaseData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var request = new DownloadTagsFromStackOverflow.Request(
            ItemsCount: 1000,
            OrderDirection: OrderDirection.Desc,
            Sort: "popular",
            Site: "stackoverflow");

        // Fire and wait synchronously during startup; exceptions will bubble up and fail fast.
        mediator.Send(request, CancellationToken.None).GetAwaiter().GetResult();

        return serviceProvider;
    }

    public static async Task<IServiceProvider> DownloadBaseDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var request = new DownloadTagsFromStackOverflow.Request(
            ItemsCount: 1000,
            OrderDirection: OrderDirection.Desc,
            Sort: "popular",
            Site: "stackoverflow");

        await mediator.Send(request, CancellationToken.None);

        return serviceProvider;
    }
}