using rest_with_asp_net10_ericles.Data.DTO.V2;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IEmailService
{
    void SendSampleEmail(string to, string subject, string body);
    Task SendEmailWithAttachment(EmailRequestDTO emailRequestDTO, IFormFile attachment);
}
