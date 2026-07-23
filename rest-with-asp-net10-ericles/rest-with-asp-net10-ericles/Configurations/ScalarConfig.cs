using Scalar.AspNetCore;

namespace rest_with_asp_net10_ericles.Configurations;

public static class ScalarConfig
{
    private static readonly string _appName = "ASP.NET REST API's With .NET10, Docker and Kubernetes";
    
    public static WebApplication UseScalarSpecification(this WebApplication app)
    {
        app.MapScalarApiReference("/scalar", options =>
        {
            options
            .WithTitle(_appName)
            .WithOpenApiRoutePattern("/swagger/v1/swagger.json");
        });
        return app;
    }
}
