using rest_with_asp_net10_ericles.Hypermedia.Abstract;

namespace rest_with_asp_net10_ericles.Hypermedia.Utils;

public class PagedSearchDTO<T> where T : ISupportsHyperMedia
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public string SortedFields { get; set; }
    public string SortDirections { get; set; } = "asc";
    public Dictionary<string, object> Filters { get; set; } = [];

    public List<T> List { get; set; } = [];

    public PagedSearchDTO()
    {
    }

    public PagedSearchDTO(int currentPage, int pageSize, string sortedFields, string sortDirections, Dictionary<string, object> filters)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        SortedFields = sortedFields;
        SortDirections = sortDirections;
        Filters = filters ?? [];
    }

    public PagedSearchDTO(int currentPage, string sortedFields, string sortDirections)
    {
        CurrentPage = currentPage;
        PageSize = 10;
        SortedFields = sortedFields;
        SortDirections = sortDirections;
        Filters = [];
    }

    public int GetCurrentPage() => CurrentPage <= 0 ? 1 : CurrentPage;

    public int GetPageSize() => PageSize <= 0 ? 10 : PageSize;
}