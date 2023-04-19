using System.Net.Mail;

namespace Shared.Implementations.Communications.Email;

public class LocalEmailRepository
{
    public async Task SendAsync(EmailDetails email, EmailOptions emailOptions)
    {
        var message = new MailMessage
        {
            From = new MailAddress(emailOptions.FromAddress),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true
        };
        foreach (var recipient in email.Recipients)
        {
            message.To.Add(new MailAddress(recipient)); 
        }

        using var client = new SmtpClient(emailOptions.Host, emailOptions.Port);
        await client.SendMailAsync(message);
    }
}