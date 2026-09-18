using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Data.DTO.V1;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.HATEOAS;

[TestMethodOrderer<PriorityOrderer>]
public class BookControllerHATEOASTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;
    private static BookDTO? _book;

    public BookControllerHATEOASTests(SqlServerFixture sqlFixture)
    {
        var factory = new CustomWebApplicationFactory<Program>(
            sqlFixture.ConnectionString);
        _httpClient = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            }
        );
    }

    private void AssertLinkPattern(string content, string rel)
    {
        var pattern =
            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v2.*?""";
        Regex.IsMatch(content, pattern).Should()
            .BeTrue($"Link with rel='{rel}' should exist and have valid href");
    }

    [Fact(DisplayName = "01 - Create Book")]
    [TestPriority(1)]
    public async Task CreateBook_ShouldContainHateoasLinks()
    {
        var request = new BookDTO
        {
            Title = "Docker Deep Dive",
            Author = "Nigel Poulton",
            Price = 54.99M,
            LaunchDate = DateTime.Now
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/book/v2", request, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _book = await response.Content.ReadFromJsonAsync<BookDTO>(
            TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "02 - Update Book")]
    [TestPriority(2)]
    public async Task UpdateBook_ShouldContainHateoasLinks()
    {
        _book!.Title = "Docker Deep Dive - 2° Edition";

        var response = await _httpClient.PutAsJsonAsync(
            "/api/book/v2", _book, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _book = await response.Content.ReadFromJsonAsync<BookDTO>(
            TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "03 - Get Book By ID")]
    [TestPriority(3)]
    public async Task GetBookById_ShouldContainHateoasLinks()
    {
        var response = await _httpClient.GetAsync(
            $"/api/book/v2/{_book!.Id}", TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _book = await response.Content.ReadFromJsonAsync<BookDTO>(
            TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "04 - Delete Book By ID")]
    [TestPriority(4)]
    public async Task DeleteBookById_ShouldReturnNoContent()
    {
        var response = await _httpClient.DeleteAsync(
            $"/api/book/v2/{_book!.Id}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
