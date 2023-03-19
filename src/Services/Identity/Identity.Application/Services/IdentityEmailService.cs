using Identity.Application.Constants;
using Identity.Application.Contracts.Services;
using Identity.Application.IdentityTemplates;
using Identity.Application.Utils;
using Microsoft.Extensions.Options;
using Shared.Implementations.Communications.Email;

namespace Identity.Application.Services;

public class IdentityEmailService : IIdentityEmailService
{
    private const string Service = "Security service";
    private readonly IEmailRepository _emailRepository;
    private readonly EmailOptions _options;

    public IdentityEmailService(IEmailRepository emailRepository, IOptions<EmailOptions> options)
    {
        _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        _options = options.Value;
    }

    public async Task SendEmailAfterCreateNewAccountAsync(string recipient, string code, string userName)
    {
        var dictData = TemplateCreator.TemplateRegisterAccount(userName, code);
        var template = Templates.GetConfirmAccountTemplate().GetTemplateReplaceData(dictData);

        await SendAsync(recipient, template, MessageSubjects.AccountConfirmation);
    }

    public async Task SendEmailResetPasswordAsync(string recipient, string resetToken, string origin)
    {
        var dictData = TemplateCreator.TemplateResetPassword(resetToken, origin);
        var template = Templates.GetResetPasswordTemplate().GetTemplateReplaceData(dictData);

        await SendAsync(recipient, template, MessageSubjects.ResetPassword);
    }

    public async Task SendEmailInitUserOrganizationAsync(string recipient, string userCode, string organizationCode)
    {
        var dictData = TemplateCreator.TemplateInitUser(userCode, organizationCode);
        var template = Templates.GetUniqueCodeTemplate().GetTemplateReplaceData(dictData);

        await SendAsync(recipient, template, MessageSubjects.Code);
    }

    private async Task SendAsync(string recipient, string template, string subject)
    {
        var mail = CreateEmailForSend(new List<string> { recipient }, Service, template, subject);
        await _emailRepository.SendEmailAsync(mail, _options);
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