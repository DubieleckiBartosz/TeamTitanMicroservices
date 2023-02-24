namespace Identity.Application.Contracts.Services;

public interface IIdentityEmailService
{
    Task SendEmailAfterCreateNewAccountAsync(string recipient, string code, string userName);
    Task SendEmailResetPasswordAsync(string recipient, string resetToken, string origin);
    Task SendEmailInitUserAsync(string recipient, string code);
}