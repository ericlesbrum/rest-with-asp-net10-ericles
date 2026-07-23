namespace rest_with_asp_net10_ericles.Configurations;

public static class RouteConfig
{
    public static IServiceCollection AddRouteConfig(this IServiceCollection services)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        return services;
    }
}
