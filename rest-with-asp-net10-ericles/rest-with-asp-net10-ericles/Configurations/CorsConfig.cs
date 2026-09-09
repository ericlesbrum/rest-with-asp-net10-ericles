using Microsoft.IdentityModel.Tokens;

namespace rest_with_asp_net10_ericles.Configurations;

public static class CorsConfig
{
    private static string[] GetAllowedOrigins(IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:Origins").Get<string[]>();
        return origins ?? Array.Empty<string>();
    }
    
    public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = GetAllowedOrigins(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy",
                policy =>
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );
        });
    }

    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        var origins = GetAllowedOrigins(configuration);

        app.Use(async (context, next) =>
        {
            var origin = context.Request.Headers["Origin"].ToString();
            if(!string.IsNullOrEmpty(origin) && !origins.Contains(origin, StringComparer.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("CORS policy not allow.");
                return;
            }
            await next();
        });

        app.UseCors("DefaultPolicy");
        return app;
    }
}
