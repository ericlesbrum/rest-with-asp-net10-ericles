using rest_with_asp_net10_ericles.Mail.Settings;

namespace rest_with_asp_net10_ericles.Configurations;

public static class EmailConfig
{
    public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Email");
        var configs = section.Get<EmailSettings>();

        if (configs == null)
            throw new ArgumentNullException(nameof(configs), "Email configuration section is missing or invalid");

        configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? configs.Username;
        configs.Username = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? configs.Password;

        services.AddSingleton(configs);

        return services;
    }
}
