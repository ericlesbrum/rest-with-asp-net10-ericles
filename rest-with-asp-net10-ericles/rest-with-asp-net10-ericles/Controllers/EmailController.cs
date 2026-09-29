using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Services.Interfaces;
using System.Text.Json;

namespace rest_with_asp_net10_ericles.Controllers
{
    [Route("api/[controller]/v2")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<BookController> _logger;

        public EmailController(IEmailService emailService, ILogger<BookController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult SendEmail([FromBody] EmailRequestDTO emailRequestDTO)
        {
            _logger.LogInformation("Sending email to {to}", emailRequestDTO.To);
            _emailService.SendSampleEmail(emailRequestDTO);
            return Ok("Email sent sucessfully");
        }

        [HttpPost("with-attachment")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendEmailWithAttachment([FromForm] string emailRequest, [FromForm] FileUploadDTO attachment)
        {
            try
            {
                if (attachment?.File == null || attachment?.File.Length == 0)
                {
                    _logger.LogWarning("Attachment is null or empty");
                    return BadRequest("Attachment is null or empty");
                }

                EmailRequestDTO? emailRequestDto = null;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                emailRequestDto = JsonSerializer.Deserialize<EmailRequestDTO>(emailRequest, options);

                if (emailRequestDto == null)
                {
                    _logger.LogWarning("Invalid email request data");
                    return BadRequest("Invalid email request data");
                }

                _logger.LogInformation("Sending email with attachment to {to}", emailRequestDto.To);
                await _emailService.SendEmailWithAttachment(emailRequestDto, attachment.File);
                return Ok("Email with attachment sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro deserializing");
                return BadRequest("Invalid email request form");
            }

        }
    }
}
