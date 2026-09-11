using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V1;
using rest_with_asp_net10_ericles.Hypermedia.Constants;

namespace rest_with_asp_net10_ericles.Hypermedia.Enricher;

public class BookEnricher : ContentResponseEnricher<BookDTO>
{
    protected override Task EnrichModel(BookDTO content, IUrlHelper urlHelper)
    {
        var request = urlHelper.ActionContext.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/book/v2";

        content.Links.Clear();
        content.Links.AddRange(GenerateLinks(content.Id, baseUrl));

        return Task.CompletedTask;
    }

    private IEnumerable<HypermediaLink> GenerateLinks(long id, string baseUrl)
    {
        return new List<HypermediaLink>
        {
            new HypermediaLink
            {
                Rel = RelationType.COLLECTION,
                Href = $"{baseUrl}",
                Type = ResponseTypeFormat.DefaultGet,
                Action = HttpActionVerb.GET,
            },
            new HypermediaLink
            {
                Rel = RelationType.SELF,
                Href = $"{baseUrl}/{id}",
                Type = ResponseTypeFormat.DefaultGet,
                Action = HttpActionVerb.GET,
            },
            new HypermediaLink
            {
                Rel = RelationType.CREATE,
                Href = $"{baseUrl}",
                Type = ResponseTypeFormat.DefaultPost,
                Action = HttpActionVerb.POST,
            },
            new HypermediaLink
            {
                Rel = RelationType.UPDATE,
                Href = $"{baseUrl}",
                Type = ResponseTypeFormat.DefaultPut,
                Action = HttpActionVerb.PUT,
            },
            new HypermediaLink
            {
                Rel = RelationType.DELETE,
                Href = $"{baseUrl}/{id}",
                Type = ResponseTypeFormat.DefaultDelete,
                Action = HttpActionVerb.DELETE,
            }
        };
    }
}
