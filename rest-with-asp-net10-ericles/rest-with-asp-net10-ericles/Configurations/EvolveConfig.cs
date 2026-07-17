using EvolveDb;
using Microsoft.Data.SqlClient;
using Serilog;

namespace rest_with_asp_net10_ericles.Configurations;

public static class EvolveConfig
{
    public static IServiceCollection AddEvolveConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            var connectionString = configuration["MSSQLSERVERSQLConnection:MSSQLSERVERSQLConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'MSSQLSERVERSQLConnection:MSSQLSERVERSQLConnectionString' not found.");
            }
            try
            {
                using var evolveConnection = new SqlConnection(connectionString);
                var evolve = new Evolve(
                    evolveConnection,
                    msg => Log.Information(msg)
                    )
                {
                    Locations = new List<string> { "db/migrations" , "db/dataset"},
                    IsEraseDisabled = true,
                    CommandTimeout = 60
                };
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "An error occurred while migrating the database.");
            }

            return services;
        }
    }
}
