using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests;

public class ScalarIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;
    public ScalarIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        var factory = new CustomWebApplicationFactory<Program>(sqlServerFixture.connectionString);
        _httpClient = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            }
        );
    }

    [Fact]
    public async Task Scalar_ShoudReturnScalarUI()
    {
        // Use current test cancellation token so test runner can cancel promptly
        var ct = TestContext.Current.CancellationToken;
        
        // Arrange & Act
        var response = await _httpClient.GetAsync("/scalar/",ct);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(ct);
        content.Should().Contain("<title>ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes</title>");
        content.Should().Contain("script src");
    }
}
