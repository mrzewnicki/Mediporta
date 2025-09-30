using MediatR;
using Mediporta.Data.Repositories;

namespace Mediporta.Core.CQRS.Command;

public static class DeleteAllTags
{
    public sealed record Request() : IRequest<Response>;

    public sealed record Response();

    public sealed class Handler(ITagsRepository tagsRepository) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var allTags = tagsRepository.GetAll(skip: 0, take: int.MaxValue);

            foreach (var tag in allTags)
            {
                // Optional: respect cancellation
                if (cancellationToken.IsCancellationRequested)
                    break;

                tagsRepository.Remove(tag);
            }

            return new Response();
        }
    }
}