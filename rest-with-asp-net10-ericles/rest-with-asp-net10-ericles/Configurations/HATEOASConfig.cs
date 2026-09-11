using rest_with_asp_net10_ericles.Hypermedia.Enricher;
using rest_with_asp_net10_ericles.Hypermedia.Filters;

namespace rest_with_asp_net10_ericles.Configurations;

public static class HATEOASConfig
{
    public static IServiceCollection AddHATEOASConfiguration(this IServiceCollection services)
    {
        var filterOptions = new HypermediaFilterOptions();
        filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
        filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

        services.AddSingleton(filterOptions);

        services.AddScoped<HypermediaFilter>();
        return services;
    }

    public static void UseHATEOASRoutes(this IEndpointRouteBuilder app)
    {
        app.MapControllerRoute("Default", "{controller=values}/v1/{id?}");
    }
}
