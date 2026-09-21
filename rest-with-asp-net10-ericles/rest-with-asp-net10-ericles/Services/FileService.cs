using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services;

public class FileService : IFileService
{
    private readonly string _basePath;
    private readonly IHttpContextAccessor _context;

    private static readonly HashSet<string> _allowedExtension = new HashSet<string> { ".jpg", ".jpeg", ".png", ".pdf", ".txt" };

    public FileService(IHttpContextAccessor context)
    {
        _context = context;
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public byte[] GetFile(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<FileDetailDTO> SaveFileToDisk(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
    {
        throw new NotImplementedException();
    }
}
