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

    public void SendSampleEmail(EmailRequestDTO emailRequestDTO)
    {
        _logger.LogInformation("Service send sample email");
        _emailSender.To(emailRequestDTO.To)
            .WithSubject(emailRequestDTO.Subject)
            .WithBody(emailRequestDTO.Body)
            .Send();
    }

    public async Task SendEmailWithAttachment(EmailRequestDTO emailRequestDTO, IFormFile attachment)
    {
        if (attachment == null || attachment.Length == 0)
        {
            _logger.LogWarning("Attachment is null or empty");
            throw new ArgumentException("Attachment is null or empty");
        }

        string tempFilePath = Path.Combine(Path.GetTempPath(), attachment.FileName);

        try
        {
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            _emailSender.To(emailRequestDTO.To)
                .WithSubject(emailRequestDTO.Subject)
                .WithBody(emailRequestDTO.Body)
                .Attachement(attachment.FileName)
                .Send();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save attachment to temp file");
            throw;
        }
        finally
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}
