using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.HATEOAS;

[TestCaseOrderer<PriorityOrderer>]
public class PersonControllerHATEOASTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;
    private static PersonDTO? _person;

    public PersonControllerHATEOASTests(SqlServerFixture sqlFixture)
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
            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v2.*?""";
        Regex.IsMatch(content, pattern).Should()
            .BeTrue($"Link with rel='{rel}' should exist and have valid href");
    }

    [Fact(DisplayName = "01 - Create Person")]
    [TestPriority(1)]
    public async Task CreatePerson_ShouldContainHateoasLinks()
    {
        var request = new PersonDTO
        {
            FirstName = "David",
            LastName = "Heinemeier",
            Address = "Copenhagen - Denmark",
            Gender = "Male",
            Enabled = true
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/person/v2", request, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);
        _person = await response.Content.ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "02 - Update Person")]
    [TestPriority(2)]
    public async Task UpdatePerson_ShouldContainHateoasLinks()
    {
        _person!.LastName = "Heinemeier Hansson";

        var response = await _httpClient.PutAsJsonAsync(
            "/api/person/v2", _person, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _person = await response.Content.ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "03 - Disable Person By Id")]
    [TestPriority(3)]
    public async Task DisablePersonById_ShouldContainHateoasLinks()
    {
        var response = await _httpClient.PatchAsync(
            $"/api/person/v2/{_person!.Id}", null, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _person = await response.Content.ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");

    }

    [Fact(DisplayName = "04 - Get Person By Id")]
    [TestPriority(4)]
    public async Task GetPersonById_ShouldContainHateoasLinks()
    {
        var response = await _httpClient.GetAsync(
            $"/api/person/v2/{_person!.Id}", TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        _person = await response.Content.ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        AssertLinkPattern(content, "collection");
        AssertLinkPattern(content, "self");
        AssertLinkPattern(content, "create");
        AssertLinkPattern(content, "update");
        AssertLinkPattern(content, "delete");
    }

    [Fact(DisplayName = "05 - Find All Persons")]
    [TestPriority(5)]
    public async Task FindAll_ShouldReturnLinksForEachPerson()
    {
        // ---------------------------
        // Arrange
        // ---------------------------
        // In this test, there is no explicit Arrange step, because
        // we are directly calling the API without preparing additional
        // data or mocking dependencies. The system under test is expected
        // to already contain one or more persons.

        // ---------------------------
        // Act
        // ---------------------------
        // Perform the HTTP GET request to retrieve all persons.
        var response = await _httpClient.GetAsync("api/person/v2", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode(); // Ensures the response status code is 2xx.

        // Read the response content as a string.
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // ---------------------------
        // Assert
        // ---------------------------
        // Extract all "id" values from the response JSON using Regex.
        var idMatches = Regex.Matches(content, @"""id"":\s*(\d+)");
        idMatches.Count.Should().BeGreaterThan(0, "There should be at least one person");

        // Iterate through each person id found in the response.
        foreach (Match match in idMatches)
        {
            var id = match.Groups[1].Value;

            // Expected hypermedia relations (HATEOAS links).
            var expectedRels = new[] { "collection", "self", "create", "update", "delete" };

            foreach (var rel in expectedRels)
            {
                // Build the expected regex pattern depending on the relation.
                // For "self" and "delete", the link must contain the specific id.
                // For others, the link points to the base endpoint.
                var pattern = rel switch
                {
                    "self" or "delete" =>
                        $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v2/{id}""",
                    _ =>
                        $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v2"""
                };

                // Assert that the link with the correct "rel" and "href" exists.
                Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase)
                     .Should().BeTrue($"Link '{rel}' should exist for person {id}");

                // Assert that each link also contains a "type" attribute.
                var typePattern = $@"""rel"":\s*""{rel}"".*?""type"":\s*""[^""]+""";
                Regex.IsMatch(content, typePattern)
                     .Should().BeTrue($"Link '{rel}' must have a type for person {id}");
            }
        }
    }
}
