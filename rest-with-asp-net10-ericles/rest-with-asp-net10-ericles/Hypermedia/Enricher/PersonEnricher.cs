using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Hypermedia.Constants;

namespace rest_with_asp_net10_ericles.Hypermedia.Enricher;

public class PersonEnricher : ContentResponseEnricher<PersonDTO>
{
    protected override Task EnrichModel(PersonDTO content, IUrlHelper urlHelper)
    {
        var request = urlHelper.ActionContext.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/person/v2";

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
                Rel = RelationType.PATCH,
                Href = $"{baseUrl}/{id}",
                Type = ResponseTypeFormat.DefaultPatch,
                Action = HttpActionVerb.PATCH,
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
