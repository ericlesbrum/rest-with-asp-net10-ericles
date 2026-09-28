using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using rest_with_asp_net10_ericles.Mail.Settings;

namespace rest_with_asp_net10_ericles.Mail;

public class EmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailSettings> _logger;

    private string _to;
    private string _subject;
    private string _body;
    private string? _attachement;
    private readonly List<MailboxAddress> _recipients = new List<MailboxAddress>();

    public EmailSender(EmailSettings emailSettings, ILogger<EmailSettings> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
    }

    public EmailSender To(string to)
    {
        _to = to;
        _recipients.Clear();
        _recipients.Add(ParseReciptients(to));
        return this;
    }

    public EmailSender WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public EmailSender WithBody(string body)
    {
        _body = body;
        return this;
    }

    public EmailSender Attachement(string filePath)
    {
        if (File.Exists(filePath))
            _attachement = filePath;
        else
            _logger.LogWarning("Attachment file not found: {FilePath}", filePath);
        return this;
    }

    public void Send()
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_emailSettings.From, _emailSettings.Username));
        message.To.AddRange(_recipients);
        message.Subject = _subject ?? _emailSettings.Subject ?? "No Subject";

        var bodyBuilder = new BodyBuilder
        {
            TextBody = _body ?? _emailSettings.Message ?? ""
        };

        if (!string.IsNullOrWhiteSpace(_attachement))
        {
            var filename = Path.GetFileName(_attachement);
            bodyBuilder.Attachments.Add(_attachement, File.ReadAllBytes(_attachement));
        }

        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            client.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.Ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

            if (_emailSettings.Properties.SmtpAuth)
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);

            client.Authenticate(_emailSettings.Username, _emailSettings.Password);
            client.Send(message);
            client.Disconnect(true);
            _logger.LogInformation("Email successfully sent to {Recipients}", string.Join(";", _recipients));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(";", _recipients));
            throw;
        }
        finally
        {
            Reset();
        }
    }

    private void Reset()
    {
        throw new NotImplementedException();
    }

    private MailboxAddress ParseReciptients(string to)
    {
        throw new NotImplementedException();
    }
}
