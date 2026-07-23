using Microsoft.OpenApi;

namespace rest_with_asp_net10_ericles.Configurations;

public static class OpenAPIConfig
{
    private static readonly string _appName = "REST with ASP.NET 10 - Ericles";
    private static readonly string _appVersion = "1.0.0";
    private static readonly string _appDescription = "RESTful API with ASP.NET 10";

    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddSingleton(new OpenApiInfo
        {
            Title = _appName,
            Version = _appVersion,
            Description = _appDescription,
            License = new OpenApiLicense
            {
                Name = "MIT"
            }
        });

        return services;
    }
}
