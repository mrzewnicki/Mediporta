using System.Diagnostics;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Mediporta.Data.Repositories;

public interface ITagsRepository
{
    IEnumerable<Tag> GetAll(IEnumerable<OrderByQuery> orderByQueries = null, int skip = 0, int take = 100);
    Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<Tag> tags);
    bool Remove(Tag tag);
}

public class TagsRepository:ITagsRepository
{
    private readonly MediportaDbContext _context;

    public TagsRepository(MediportaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Tag> GetAll(IEnumerable<OrderByQuery> orderByQueries = null, int skip = 0, int take = 100)
    {
        if(orderByQueries is null || !orderByQueries.Any())
            orderByQueries = new List<OrderByQuery> { new(t => t.Name) };

        var firstOrderQuery = orderByQueries.First();

        var query = firstOrderQuery.IsAscending
            ? _context.Tags.OrderBy(firstOrderQuery.KeySelector)
            : _context.Tags.OrderByDescending(firstOrderQuery.KeySelector);

        if (orderByQueries.Count() > 1)
        {
            foreach (var orderByQuery in orderByQueries.Skip(1))
            {
                query = orderByQuery.IsAscending
                    ? query.ThenBy(orderByQuery.KeySelector)
                    : query.ThenByDescending(orderByQuery.KeySelector);
            }
        }

        return query.Skip(skip)
            .Take(take)
            .AsEnumerable();
    }

    public async Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Tag> tags)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Tags.AddRange(tags);
            var result = await _context.SaveChangesAsync();

            if(result != tags.Count())
                throw new Exception("Not all tags were added successfully.");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // ToDo: logger
            throw;
        }
    }

    public bool Remove(Tag tag)
    {
        _context.Remove(tag);
        return _context.SaveChanges() > 0;
    }
}