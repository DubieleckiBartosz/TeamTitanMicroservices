namespace Shared.Implementations.Communications.Email;

public interface IEmailRepository
{
    Task SendEmailAsync(EmailDetails email, EmailOptions emailOptions);
    Task SendAsync(string recipient, string template, string subject, string serviceName, EmailOptions emailOptions);
}