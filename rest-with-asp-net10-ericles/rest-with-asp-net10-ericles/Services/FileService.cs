using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services;

public class FileService : IFileService
{
    private readonly string _basePath;
    private readonly IHttpContextAccessor _context;

    private static readonly HashSet<string> _allowedExtensions = new() { ".txt", ".pdf", ".png", ".jpg", ".jpeg", ".docx", ".mp3" };

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
        var filePath = Path.Combine(_basePath, fileName);
        if (!File.Exists(filePath))
            return null;
        return File.ReadAllBytes(filePath);
    }

    public async Task<FileDetailDTO> SaveFileToDisk(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty or null");

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if(!_allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("File type is not allowed");

        var documentName = Path.GetFileName(file.FileName);
        var destination = Path.Combine(_basePath, documentName);

        var baseUrl = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";

        var fileDetail = new FileDetailDTO
        {
            DocumentName = documentName,
            DocType = file.ContentType,
            DocUrl = $"{baseUrl}/api/file/v2/downloadFile/{documentName}"
        };

        using (var stream = new FileStream(destination, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileDetail;
    }

    public async Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
    {
        var results  = new List<FileDetailDTO>();
        foreach (var file in files)
        {
            var fileDetail = await SaveFileToDisk(file);
            if(!string.IsNullOrEmpty(fileDetail.DocumentName))
                results.Add(fileDetail);
        }
        return results;
    }
}
