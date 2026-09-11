using rest_with_asp_net10_ericles.Hypermedia.Abstract;

namespace rest_with_asp_net10_ericles.Hypermedia.Filters;

public class HypermediaFilterOptions
{
    public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = [];
}
