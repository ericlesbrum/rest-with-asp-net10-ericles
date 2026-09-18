using FluentAssertions;
using rest_with_asp_net10_ericles.Repositories.QueryBuilders;
using System.Text.RegularExpressions;

namespace rest_with_asp_net10_ericles.Tests.UnitTests;

public class PersonQueryBuilderTests
{
    private readonly PersonQueryBuilder _queryBuilder;

    public PersonQueryBuilderTests()
    {
        _queryBuilder = new PersonQueryBuilder();
    }

    private static string Normalize(string sql) => Regex.Replace(sql, @"\s+", " ").Trim();

    [Fact]
    public void BuildQueries_ShouldReturnCorrectQueries()
    {
        //Arrage
        var name = "John";
        var sortDirection = "asc";
        var pageSize = 10;
        var page = 2;

        //Act
        var (query,countQuery,sort,size,offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

        //Assert
        var q = Normalize(query);
        q.Should().Contain("SELECT * FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%John%')");
        q.Should().Contain("ORDER BY p.first_name asc");
        q.Should().Contain("OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY");

        Normalize(countQuery).Should()
        .Be("SELECT COUNT(*) FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%John%')");

        sort.Should().Be("asc");
        size.Should().Be(10);
        offset.Should().Be(10);
    }

    [Fact]
    public void BuildQueries_ShouldHandleInvalidPageSizeAndPage()
    {
        //Arrange
        var name = "Jane";
        var sortDirection = "desc";
        var pageSize = 0; // Invalid page size
        var page = -1;    // Invalid page number
        
        //Act
        var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
        
        //Assert
        var q = Normalize(query);
        q.Should().Contain("SELECT * FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%Jane%')");
        q.Should().Contain("ORDER BY p.first_name desc");
        q.Should().Contain("OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY"); // Default to 1 row
        
        Normalize(countQuery).Should()
            .Be("SELECT COUNT(*) FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%Jane%')");
        sort.Should().Be("desc");
        size.Should().Be(1); // Default to 1
        offset.Should().Be(0); // Default to 0
    }

    [Fact]
    public void BuildQueries_ShouldHandleNullOrWhitespaceName()
    {
        //Arrange
        string name = null; // Null name
        var sortDirection = "asc";
        var pageSize = 5;
        var page = 1;
        
        //Act
        var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
        
        //Assert
        var q = Normalize(query);
        q.Should().Contain("SELECT * FROM dbo.person p WHERE 1=1");
        q.Should().Contain("ORDER BY p.first_name asc");
        q.Should().Contain("OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY");
        q.Should().NotContain("AND (p.first_name LIKE '%");

        Normalize(countQuery).Should()
            .Be("SELECT COUNT(*) FROM dbo.person p WHERE 1=1");
        sort.Should().Be("asc");
        size.Should().Be(5);
        offset.Should().Be(0);
    }

    [Fact]
    public void BuildQueries_ShouldDefaultToDescForInvalidDirection()
    {
        //Arrange
        var name = "Alice";
        var sortDirection = "invalid"; // Invalid sort direction
        var pageSize = 10;
        var page = 1;

        //Act
        var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

        //Assert
        var q = Normalize(query);
        q.Should().Contain("SELECT * FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%Alice%')");
        q.Should().Contain("ORDER BY p.first_name asc"); // Default to asc
        q.Should().Contain("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY");

        Normalize(countQuery).Should()
            .Be("SELECT COUNT(*) FROM dbo.person p WHERE 1=1 AND (p.first_name LIKE '%Alice%')");
        sort.Should().Be("asc"); // Default to asc
        size.Should().Be(10);
        offset.Should().Be(0);
    }
}
