using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Services.Interfaces;

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
        [ProducesResponseType(200,Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult SendEmail([FromBody] EmailRequestDTO emailRequestDTO)
        {
            _logger.LogInformation("Sending email to {to}", emailRequestDTO.To);
            _emailService.SendSampleEmail(emailRequestDTO);
            return Ok("Email sent sucessfully");
        }
    }
}
