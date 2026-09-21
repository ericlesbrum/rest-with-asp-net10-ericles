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
}
