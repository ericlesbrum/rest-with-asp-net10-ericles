using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Json;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.Person.JSON;

[TestCaseOrderer<PriorityOrderer>]
public class PersonControllerJsonTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;
    private static PersonDTO _person = null!;

    public PersonControllerJsonTests(SqlServerFixture sqlFixture)
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

    [Fact(DisplayName = "01 - Create Person")]
    [TestPriority(1)]
    public async Task CreatePerson_ShouldReturnCreatedPerson()
    {
        // Arrange
        var request = new PersonDTO
        {
            FirstName = "Linus",
            LastName = "Torvalds",
            Address = "Helsinki - Finland",
            Gender = "Male",
            Enabled = true
        };

        // Act
        var response = await _httpClient
            .PostAsJsonAsync("api/person/v2", request,TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var created = await response.Content
            .ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);
        created.FirstName.Should().Be("Linus");
        created.LastName.Should().Be("Torvalds");
        created.Address.Should().Be("Helsinki - Finland");
        created.Enabled.Should().BeTrue();

        _person = created;
    }

    [Fact(DisplayName = "02 - Update Person")]
    [TestPriority(2)]
    public async Task UpdatePerson_ShouldReturnUpdatedPerson()
    {
        // Arrange
        _person.LastName = "Benedict Torvalds";

        // Act
        var response = await _httpClient
            .PutAsJsonAsync("api/person/v2", _person, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var updated = await response.Content
                .ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        updated.Should().NotBeNull();
        updated.Id.Should().BeGreaterThan(0);
        updated.FirstName.Should().Be("Linus");
        updated.LastName.Should().Be("Benedict Torvalds");
        updated.Address.Should().Be("Helsinki - Finland");
        updated.Enabled.Should().BeTrue();

        _person = updated;
    }

    [Fact(DisplayName = "03 - Disable Person By ID")]
    [TestPriority(3)]
    public async Task DisablePersonById_ShouldReturnDisabledPerson()
    {
        // Arrange & Act
        var response = await _httpClient
            .PatchAsync($"api/person/v2/{_person.Id}", null, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var disabled = await response.Content
                .ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        disabled.Should().NotBeNull();
        disabled.Id.Should().BeGreaterThan(0);
        disabled.FirstName.Should().Be("Linus");
        disabled.LastName.Should().Be("Benedict Torvalds");
        disabled.Address.Should().Be("Helsinki - Finland");
        disabled.Enabled.Should().BeFalse();

        _person = disabled;
    }

    [Fact(DisplayName = "04 - Get Person By ID")]
    [TestPriority(4)]
    public async Task GetPersonById_ShouldReturnPerson()
    {
        // Arrange & Act
        var response = await _httpClient
            .GetAsync($"api/person/v2/{_person.Id}", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var found = await response.Content
            .ReadFromJsonAsync<PersonDTO>(TestContext.Current.CancellationToken);

        found.Should().NotBeNull();
        found.Id.Should().Be(_person.Id);
        found.FirstName.Should().Be("Linus");
        found.LastName.Should().Be("Benedict Torvalds");
        found.Address.Should().Be("Helsinki - Finland");
        found.Enabled.Should().BeFalse();
    }

    [Fact(DisplayName = "05 - Delete Person By ID")]
    [TestPriority(5)]
    public async Task DeletePersonById_ShouldReturnNoContent()
    {
        // Arrange & Act
        var response = await _httpClient
            .DeleteAsync($"api/person/v2/{_person.Id}", TestContext.Current.CancellationToken);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "06 - Find all Person")]
    [TestPriority(6)]
    public async Task FindAllPerson_ShouldReturnListOfPerson()
    {
        // Arrange & Act
        var response = await _httpClient
            .GetAsync("api/person/v2", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        var list = await response.Content
                .ReadFromJsonAsync<List<PersonDTO>>(TestContext.Current.CancellationToken);

        list.Should().NotBeNull();
        list.Count.Should().BeGreaterThan(0);

        var first = list.First(p => p.FirstName == "Ayrton");
        first.LastName.Should().Be("Senna");
        first.Address.Should().Be("São Paulo - Brasil");
        first.Enabled.Should().BeTrue();
        first.Gender.Should().Be("Male");

        var fifth = list.First(p => p.FirstName == "Ada");
        fifth.LastName.Should().Be("Lovelace");
        fifth.Address.Should().Be("London - England");
        fifth.Enabled.Should().BeTrue();
        fifth.Gender.Should().Be("Female");
    }
}
