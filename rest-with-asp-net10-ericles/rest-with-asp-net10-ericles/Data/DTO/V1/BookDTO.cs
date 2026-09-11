using rest_with_asp_net10_ericles.Hypermedia;
using rest_with_asp_net10_ericles.Hypermedia.Abstract;

namespace rest_with_asp_net10_ericles.Data.DTO.V1;

public class BookDTO : ISupportsHyperMedia
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public DateTime LaunchDate { get; set; }
    public List<HypermediaLink> Links { get; set; } = [];
}
