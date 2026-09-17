using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using rest_with_asp_net10_ericles.Hypermedia.Abstract;
using rest_with_asp_net10_ericles.Hypermedia.Utils;

namespace rest_with_asp_net10_ericles.Hypermedia;

public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportsHyperMedia
{
    public virtual bool CanEnrich(Type contentType)
    {
        return typeof(T).IsAssignableFrom(contentType)
            || typeof(IEnumerable<T>).IsAssignableFrom(contentType)
            || typeof(PagedSearchDTO<T>).IsAssignableFrom(contentType);
    }

    public async Task Enrich(ResultExecutingContext response)
    {
        var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
        if (response.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is T content)
            {
                await EnrichModel(content, urlHelper);
            }
            else if (objectResult.Value is IEnumerable<T> contentList)
            {
                foreach (var item in contentList)
                {
                    await EnrichModel(item, urlHelper);
                }
            }
            else if (objectResult.Value is PagedSearchDTO<T> pagedSearch)
            {
                foreach (var item in pagedSearch.List)
                {
                    item.Links?.Clear();
                    await EnrichModel(item, urlHelper);
                }
            }
            await Task.CompletedTask;
        }
    }

    protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

    bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
    {
        if (response.Result is ObjectResult objectResult)
        {
            var contentType = objectResult.Value?.GetType();
            return contentType != null ? CanEnrich(contentType) : false;
        }
        return false;
    }
}
