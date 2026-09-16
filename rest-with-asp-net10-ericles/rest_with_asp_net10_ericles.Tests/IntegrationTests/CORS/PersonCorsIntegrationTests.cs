using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Json;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.CORS;

[TestCaseOrderer<PriorityOrderer>]
public class PersonCorsIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;
    private static PersonDTO _person = null!;

    public PersonCorsIntegrationTests(SqlServerFixture sqlFixture)
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

    private void AddOriginHeader(string origin)
    {
        _httpClient.DefaultRequestHeaders.Remove("Origin");
        _httpClient.DefaultRequestHeaders.Add("Origin", origin);
    }

    [Fact(DisplayName = "01 - Create Person With Allowed Origin")]
    [TestPriority(1)]
    public async Task CreatePerson_WithAllowedOrigin_ShouldReturnCreated()
    {
        // Arrange
        AddOriginHeader("https://erudio.com.br");

        var request = new PersonDTO
        {
            FirstName = "Richard",
            LastName = "Stallman",
            Address = "New York City - New York - USA",
            Gender = "Male"
        };

        // Act
        var response = await _httpClient
            .PostAsJsonAsync(
                "api/person/v2",
                request,
                TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var created = await response.Content
            .ReadFromJsonAsync<PersonDTO>(
                TestContext.Current.CancellationToken);
        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);

        _person = created;
    }

    [Fact(DisplayName = "02 - Create Person With Disallowed Origin")]
    [TestPriority(2)]
    public async Task CreatePerson_WithDisallowedOrigin_ShouldReturnForbiden()
    {
        // Arrange
        AddOriginHeader("https://semeru.com.br");

        var request = new PersonDTO
        {
            FirstName = "Richard",
            LastName = "Stallman",
            Address = "New York City - New York - USA",
            Gender = "Male"
        };

        // Act
        var response = await _httpClient
            .PostAsJsonAsync(
                "api/person/v2",
                request,
                TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        var content = await response.Content
            .ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("CORS origin not allowed.");
    }

    [Fact(DisplayName = "03 - Get Person By ID With Allowed Origin")]
    [TestPriority(3)]
    public async Task FindPersonById_WithAllowedOrigin_ShouldReturnOk()
    {
        // Arrange
        AddOriginHeader("https://erudio.com.br");

        // Act
        var response = await _httpClient
            .GetAsync(
                $"api/person/v2/{_person.Id}",
                TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var found = await response.Content
            .ReadFromJsonAsync<PersonDTO>(
                TestContext.Current.CancellationToken);

        found.Should().NotBeNull();
        found.Id.Should().Be(_person.Id);
        found.FirstName.Should().Be("Richard");
        found.LastName.Should().Be("Stallman");
        found.Address.Should().Be("New York City - New York - USA");
    }

    [Fact(DisplayName = "04 - Get Person By ID With Disallowed Origin")]
    [TestPriority(4)]
    public async Task FindByIdPerson_WithDisallowedOrigin_ShouldReturnForbiden()
    {
        // Arrange
        AddOriginHeader("https://semeru.com.br");

        // Act
        var response = await _httpClient
            .GetAsync(
                $"api/person/v2/{_person.Id}",
                TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var content = await response.Content
            .ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("CORS origin not allowed.");
    }

}
