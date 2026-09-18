namespace rest_with_asp_net10_ericles.Data.DTO.V2;

public class PagedSearch<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string SortDirections { get; set; } = "asc";
    public int TotalResults { get; set; }
    public List<T> List { get; set; } = [];
}