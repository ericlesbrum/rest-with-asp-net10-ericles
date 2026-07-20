using Serilog;

namespace rest_with_asp_net10_ericles;

public static class LoggingConfig
{
  public static void AddSerilogLogging(this WebApplicationBuilder builder)
  {
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Debug()
      .CreateLogger();

      builder.Host.UseSerilog();
  }
}
