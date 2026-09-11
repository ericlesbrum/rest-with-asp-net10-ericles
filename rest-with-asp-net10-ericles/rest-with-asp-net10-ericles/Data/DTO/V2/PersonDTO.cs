using rest_with_asp_net10_ericles.Hypermedia;
using rest_with_asp_net10_ericles.Hypermedia.Abstract;

namespace rest_with_asp_net10_ericles.Data.DTO.V2;

public class PersonDTO : ISupportsHyperMedia
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool Enabled { get; set; }
    public List<HypermediaLink> Links { get; set; } = [];
}
