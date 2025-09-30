using FluentAssertions;
using Mediporta.Shared.Querying;
using Xunit;

namespace Mediporta.Tests.Unit;

public class QueryOptionsTests
{
    [Fact]
    public void Build_Should_Parse_Multiple_Sorts()
    {
        var options = QueryOptions.Build("Name:asc,PercentageOfAll:desc", page: 2, pageSize: 25);
        options.Sorts.Should().HaveCount(2);
        options.Sorts[0].Field.Should().Be("Name");
        options.Sorts[0].Direction.ToString().Should().Be("Asc");
        options.Sorts[1].Field.Should().Be("PercentageOfAll");
        options.Sorts[1].Direction.ToString().Should().Be("Desc");
        options.Page.Should().Be(2);
        options.PageSize.Should().Be(25);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Build_Should_Handle_Empty_Sort(string? sort)
    {
        var options = QueryOptions.Build(sort!, page: 1, pageSize: 10);
        options.Sorts.Should().BeEmpty();
    }

    [Theory]
    [InlineData("asc", "Asc")]
    [InlineData("desc", "Desc")]
    [InlineData("unknown", "Asc")]
    public void Build_Should_Fallback_Unknown_Direction_To_Asc(string input, string expected)
    {
        var options = QueryOptions.Build($"Name:{input}", page: 1, pageSize: 10);
        options.Sorts.Should().ContainSingle();
        options.Sorts[0].Direction.ToString().Should().Be(expected);
    }

    [Fact]
    public void Build_Should_Normalize_Page_And_PageSize()
    {
        var options = QueryOptions.Build("Name:asc", page: 0, pageSize: 0);
        options.Page.Should().Be(1);
        options.PageSize.Should().BeGreaterThan(0);
    }
}