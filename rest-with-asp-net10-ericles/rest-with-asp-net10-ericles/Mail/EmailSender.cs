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
        _recipients.Add(ParseReciptients(to);
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

    private MailboxAddress ParseReciptients(string to)
    {
        throw new NotImplementedException();
    }
}
