using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Controllers;

[Route("api/[controller]/v2")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost("uploadFile")]
    [ProducesResponseType(200, Type = typeof(FileDetailDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json","application/xml")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO input)
    {
        var fileDetail = await _fileService.SaveFileToDisk(input.File);
        _logger.LogInformation($"File {fileDetail.DocumentName} uploaded successfully.");
        return Ok(fileDetail);
    }

    [HttpPost("uploadMultipleFiles")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(200, Type = typeof(List<FileDetailDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json", "application/xml")]
    public async Task<IActionResult> UploadMultipleFiles([FromForm] MultipleFilesUploadDTO input)
    {
        var fileDetail = await _fileService.SaveFilesToDisk(input.Files);
        _logger.LogInformation($"Files {fileDetail} uploaded successfully.");
        return Ok(fileDetail);
    }

    [HttpGet("downloadFile/{fileName}")]
    [ProducesResponseType(200, Type = typeof(byte[]))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var buffer = _fileService.GetFile(fileName);
        if (buffer == null || buffer.Length == 0)
            return NoContent();

        var contentType = $"application/{Path.GetExtension(fileName).TrimStart('.')}";
        return File(buffer, contentType, fileName);
    }
}
