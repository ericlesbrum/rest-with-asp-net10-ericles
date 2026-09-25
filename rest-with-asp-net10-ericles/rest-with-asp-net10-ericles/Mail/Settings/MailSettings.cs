namespace rest_with_asp_net10_ericles.Mail.Settings;

public class MailSettings
{
    public bool SmtpAuth { get; set; }
    public bool StartTlsEnable { get; set; }
    public bool StartTlsRequired { get; set; }

    public MailSettings()
    {
    }
}