using Microsoft.AspNetCore.Mvc.Filters;

namespace rest_with_asp_net10_ericles.Hypermedia.Abstract;

public interface IResponseEnricher
{
    bool CanEnrich(ResultExecutingContext context);
    Task Enrich(ResultExecutingContext context);
}
