using Microsoft.EntityFrameworkCore;
using rest_with_asp_net10_ericles.Model.Context;

namespace rest_with_asp_net10_ericles.Configurations;

public static class DatabaseConfig
{
  public static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration["MSSQLSERVERSQLConnection:MSSQLSERVERSQLConnectionString"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new InvalidOperationException("Connection string 'MSSQLSERVERSQLConnection:MSSQLSERVERSQLConnectionString' not found.");
    }
    services.AddDbContext<MSSQLContext>(options => options.UseSqlServer(connectionString));
    return services;
  }
}
