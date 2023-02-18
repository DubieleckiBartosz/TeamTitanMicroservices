namespace Shared.Implementations.Communications.Email;

public interface IEmailRepository
{
    Task SendEmailAsync(EmailDetails email, EmailOptions emailOptions);
}