using rest_with_asp_net10_ericles.Data.DTO.V2;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IFileService
{
    byte[] GetFile(string fileName);
    Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
    Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
}
