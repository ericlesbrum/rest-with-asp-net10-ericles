using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using rest_with_asp_net10_ericles.Hypermedia.Abstract;

namespace rest_with_asp_net10_ericles.Hypermedia;

public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportsHyperMedia
{
    public virtual bool CanEnrich(Type contentType)
    {
        return contentType == typeof(T) || contentType == typeof(T[]);
    }

    public async Task Enrich(ResultExecutingContext response)
    {
        var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
        if (response.Result is OkObjectResult okObjectResult)
        {
            if (okObjectResult.Value is T content)
            {
                await EnrichModel(content, urlHelper);
            }
            else if (okObjectResult.Value is IEnumerable<T> contentList)
            {
                foreach (var item in contentList)
                {
                    await EnrichModel(item, urlHelper);
                }
            }
            await Task.CompletedTask;
        }
    }

    protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

    bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
    {
        if (response.Result is OkObjectResult okObjectResult)
        {
            var contentType = okObjectResult.Value?.GetType();
            return contentType != null ? CanEnrich(contentType) : false;
        }
        return false;
    }
}
