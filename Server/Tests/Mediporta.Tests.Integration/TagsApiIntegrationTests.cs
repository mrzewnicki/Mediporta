using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Mediporta.Api.Controllers;
using Mediporta.Data;
using Mediporta.Data.Entities;
using Mediporta.Shared.DTOs;
using Mediporta.StackOverflow.ApiClient;
using Mediporta.StackOverflow.ApiClient.Models.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Mediporta.Tests.Integration;

public class TagsApiIntegrationTests : IClassFixture<WebApplicationFactory<TagsController>>
{
    private readonly WebApplicationFactory<TagsController> _factory;

    public TagsApiIntegrationTests(WebApplicationFactory<TagsController> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MediportaDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Use SQLite with a unique file per test run and apply migrations
                var dbPath = Path.Combine(Path.GetTempPath(), $"mediporta_it_{Guid.NewGuid():N}.db");
                services.AddDbContext<MediportaDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));

                // Override external API client to avoid real HTTP calls during startup seeding
                services.AddSingleton<IStackOverflowApiClient>(new FakeStackOverflowApiClient());
            });
        });
    }

    [Fact]
    public async Task GetAll_Should_Return_Paginated_And_Sorted()
    {
        var client = _factory.CreateClient();

        // Seed test data after host startup and migrations
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<MediportaDbContext>();
            if (!ctx.Tags.Any())
            {
                ctx.Tags.AddRange(new List<Tag>
                {
                    new (){ Name = "b", Count = 1, PercentageOfAll = 33 },
                    new (){ Name = "a", Count = 2, PercentageOfAll = 67 },
                });
                
                ctx.SaveChanges();
            }
        }

        var result = await client.GetFromJsonAsync<GetTagResultDto>($"api/Tags/GetAll?sort={nameof(Tag.Name)}:asc&page=1&pageSize=10");

        result.Should().NotBeNull();
        result!.TotalCount.Should().Be(2);
        result.Tags.Select(t => t.Name).Should().ContainInOrder("a", "b");
    }

    private class FakeStackOverflowApiClient : IStackOverflowApiClient
    {
        public Task<IEnumerable<Mediporta.StackOverflow.ApiClient.Models.Tag>> GetTagsAsync(GetTags.Request request)
            => Task.FromResult<IEnumerable<Mediporta.StackOverflow.ApiClient.Models.Tag>>(Array.Empty<Mediporta.StackOverflow.ApiClient.Models.Tag>());
    }
}