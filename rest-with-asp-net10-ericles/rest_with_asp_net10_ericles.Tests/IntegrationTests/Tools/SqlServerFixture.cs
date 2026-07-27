using DotNet.Testcontainers.Images;
using rest_with_asp_net10_ericles.Configurations;
using Testcontainers.MsSql;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;

public class SqlServerFixture : IAsyncLifetime
{
    public MsSqlContainer SqlContainer { get;}
    public string connectionString => SqlContainer.GetConnectionString();

    public SqlServerFixture()
    {
        IImage image = new DockerImage("mcr.microsoft.com/mssql/server:2025-CU6-ubuntu-22.04");
        SqlContainer = new MsSqlBuilder(image).WithPassword("@Admin123").Build();
    }


    public async ValueTask InitializeAsync()
    {
        await SqlContainer.StartAsync();
        EvolveConfig.ExecuteMigrations(connectionString);
    }
    
    public async ValueTask DisposeAsync()
    {
        await SqlContainer.DisposeAsync();
    }
}