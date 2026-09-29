using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Mail;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services;

public class EmailService : IEmailService
{
    private readonly EmailSender _emailSender;
    private readonly ILogger<EmailService> _logger;

    public EmailService(EmailSender emailSender, ILogger<EmailService> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public void SendSampleEmail(string to, string subject, string body)
    {
        _logger.LogInformation("Service send sample email");
        _emailSender.To(to)
            .WithSubject(subject)
            .WithBody(body)
            .Send();
    }
    public Task SendEmailWithAttachment(EmailRequestDTO emailRequestDTO, IFormFile attachment)
    {
        throw new NotImplementedException();
    }

}
