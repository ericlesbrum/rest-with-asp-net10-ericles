using System.ComponentModel.DataAnnotations;

namespace rest_with_asp_net10_ericles.Data.DTO.V2;

public class FileUploadDTO
{
    [Required]
    public IFormFile File { get; set; }
}
