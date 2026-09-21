using System.ComponentModel.DataAnnotations;

namespace rest_with_asp_net10_ericles.Data.DTO.V2;

public class MultipleFilesUploadDTO
{
    [Required]
    public List<IFormFile> Files { get; set; }
}
