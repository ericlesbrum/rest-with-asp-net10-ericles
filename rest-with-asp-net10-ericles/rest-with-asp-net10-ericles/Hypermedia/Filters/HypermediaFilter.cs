using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace rest_with_asp_net10_ericles.Hypermedia.Filters;

public class HypermediaFilter(HypermediaFilterOptions hypermediaFilterOptions) : ResultFilterAttribute
{
    private readonly HypermediaFilterOptions _hypermediaFilterOptions = hypermediaFilterOptions;
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        TryEnrichResult(context);
        base.OnResultExecuting(context);
    }

    private void TryEnrichResult(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && IsSuccessStatusCode(objectResult))
        {
            var enricher = _hypermediaFilterOptions.ContentResponseEnricherList
                .FirstOrDefault(option => option.CanEnrich(context));
            enricher?.Enrich(context).Wait();
        }
    }

    private static bool IsSuccessStatusCode(ObjectResult result)
    {
        var statusCode = result.StatusCode ?? StatusCodes.Status200OK;
        return statusCode >= StatusCodes.Status200OK && statusCode < StatusCodes.Status300MultipleChoices;
    }
}
