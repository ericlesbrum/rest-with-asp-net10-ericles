using Microsoft.OpenApi;

namespace rest_with_asp_net10_ericles.Configurations;

public static class SwaggerConfig
{
    private static readonly string _appName = "ASP.NET REST API's With .NET10, Docker and Kubernetes";
    private static readonly string _appVersion = "1.0.0";
    private static readonly string _appDescription = "REST API Restful";

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = _appName,
                Version = _appVersion,
                Description = _appDescription,
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });

            options.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerSpecification(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json","v1");
            options.RoutePrefix = "swagger-ui";
            options.DocumentTitle = _appName;
        });
        return app;
    }
}
