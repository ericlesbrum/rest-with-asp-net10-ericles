namespace rest_with_asp_net10_ericles.Data.DTO.V2;

public class EmailRequestDTO
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
