using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mediporta.Data;
using Mediporta.Data.Entities;
using Mediporta.Data.Repositories;
using Mediporta.Data.Repositories.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Mediporta.Tests.Unit;

public class TagsRepositoryTests
{
    private static MediportaDbContext CreateContext()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"mediporta_unit_{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<MediportaDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        var ctx = new MediportaDbContext(options);
        ctx.Database.Migrate();
        return ctx;
    }

    [Fact]
    public async Task Remove_Should_Delete_Record()
    {
        using var ctx = CreateContext();
        var repo = new TagsRepository(ctx);
        var tag = new Tag { Name = "x" };
        await repo.AddRangeAsync(new[] { tag });

        var toRemove = ctx.Tags.Single();
        var result = repo.Remove(toRemove);
        result.Should().BeTrue();
        ctx.Tags.Should().BeEmpty();
    }
}