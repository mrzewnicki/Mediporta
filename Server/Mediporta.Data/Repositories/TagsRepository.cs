using System.Diagnostics;
using Mediporta.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mediporta.Data.Repositories;

public interface ITagsRepository
{
    Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken);
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

    public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tags.ToListAsync(cancellationToken);
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