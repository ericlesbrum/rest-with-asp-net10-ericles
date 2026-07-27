using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests;

public class SwaggerIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _httpClient;

    public SwaggerIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        var factory = new CustomWebApplicationFactory<Program>(sqlServerFixture.connectionString);
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost")
        });
    }

    [Fact]
    public async Task Get_SwaggerJson_ShouldReturnSwaggerJson()
    {
        // Use current test cancellation token so test runner can cancel promptly
        var ct = TestContext.Current.CancellationToken;

        // Arrange & Act
        var response = await _httpClient.GetAsync("/swagger/v1/swagger.json", ct);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        content.Should().NotBeNull();
        content.Should().Contain("/api/person/v2");
    }

    [Fact]
    public async Task SwaggerUI_ShouldReturnSwaggerUI()
    {
        // Use current test cancellation token so test runner can cancel promptly
        var ct = TestContext.Current.CancellationToken;

        // Arrange & Act
        var response = await _httpClient.GetAsync("/swagger-ui/index.html", ct);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        content.Should().NotBeNull();
        content.Should().Contain("<div id=\"swagger-ui\">");
    }
}
