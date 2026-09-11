namespace rest_with_asp_net10_ericles.Hypermedia.Abstract;

public interface ISupportsHyperMedia
{
    List<HypermediaLink> Links { get; set; }
}
