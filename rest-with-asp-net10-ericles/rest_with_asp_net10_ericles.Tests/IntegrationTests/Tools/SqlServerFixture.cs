using DotNet.Testcontainers.Images;
using rest_with_asp_net10_ericles.Configurations;
using System.ComponentModel;
using Testcontainers.MsSql;

namespace rest_with_asp_net10_ericles.Tests.IntegrationTests.Tools;

public class SqlServerFixture : IAsyncLifetime
{
    public MsSqlContainer SqlContainer { get;}
    public string ConnectionString => SqlContainer.GetConnectionString();

    public SqlServerFixture()
    {
        var image = new DockerImage("mcr.microsoft.com/mssql/server:2025-CU6-ubuntu-22.04");
        SqlContainer = new MsSqlBuilder(image)
            .WithPassword("@Admin123")
            .WithPortBinding(0, 1433)
            .Build();
    }


    public async ValueTask InitializeAsync()
    {
        await SqlContainer.StartAsync();
        EvolveConfig.ExecuteMigrations(ConnectionString);
    }
    
    public async ValueTask DisposeAsync()
    {
        await SqlContainer.DisposeAsync();
    }
}