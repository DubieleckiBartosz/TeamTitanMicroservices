using MimeKit.Text;
using MimeKit;
using Shared.Implementations.Logging;
using System.Net;
using MailKit.Net.Smtp; 
using Microsoft.Extensions.Configuration;
using Shared.Implementations.Core.Exceptions; 

namespace Shared.Implementations.Communications.Email;

public class EmailRepository : IEmailRepository
{
    private readonly ILoggerManager<EmailRepository> _loggerManager;
    private readonly IConfiguration _configuration;

    public EmailRepository(ILoggerManager<EmailRepository> loggerManager, IConfiguration configuration)
    {
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task SendEmailAsync(EmailDetails email, EmailOptions emailOptions)
    { 
        if (_configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
        { 
            var localEmailRepository = new LocalEmailRepository();

            await localEmailRepository.SendAsync(email, emailOptions);

            _loggerManager.LogInformation($"Send local email.");

            return;
        }
         
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(email.FromName ?? emailOptions.FromName, emailOptions.FromAddress));

        foreach (var recipient in email.Recipients)
        {
            mailMessage.To.Add(MailboxAddress.Parse(recipient));
            _loggerManager.LogInformation($"Recipient: {recipient}");
        }

        mailMessage.Subject = email.Subject;
        mailMessage.Body = new TextPart(TextFormat.Html) { Text = email.Body };

        try
        { 
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await smtpClient.ConnectAsync(emailOptions.Host, emailOptions.Port, true);
                await smtpClient.AuthenticateAsync(emailOptions.User, emailOptions.Password);
                await smtpClient.SendAsync(mailMessage);
                await smtpClient.DisconnectAsync(true);
            }

            _loggerManager.LogInformation($"Send email.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"From email service: {ex.Message}");
            throw new TeamTitanApplicationException("Email failed", "Email Exception",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task SendAsync(string recipient, string template, string subject, string serviceName, EmailOptions emailOptions)
    {
        var mail = CreateEmailForSend(new List<string> { recipient }, serviceName, template, subject);
        await SendEmailAsync(mail, emailOptions);
    }
    private EmailDetails CreateEmailForSend(List<string> recipients, string from, string body, string subjectMail)
    {
        return new EmailDetails
        {
            Body = body,
            Subject = subjectMail,
            Recipients = recipients,
            FromName = from
        };
    }
}