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

        await _emailRepository.SendAsync(recipient, template, MessageSubjects.AccountConfirmation, Service, _options);
    }

    public async Task SendEmailResetPasswordAsync(string recipient, string resetToken, string origin)
    {
        var dictData = TemplateCreator.TemplateResetPassword(resetToken, origin);
        var template = Templates.GetResetPasswordTemplate().GetTemplateReplaceData(dictData);

        await _emailRepository.SendAsync(recipient, template, MessageSubjects.ResetPassword, Service, _options);
    }

    public async Task SendEmailInitUserOrganizationAsync(string recipient, string userCode, string organizationCode)
    {
        var dictData = TemplateCreator.TemplateInitUser(userCode, organizationCode);
        var template = Templates.GetUniqueCodeTemplate().GetTemplateReplaceData(dictData);

        await _emailRepository.SendAsync(recipient, template, MessageSubjects.Code, Service, _options);
    } 
}