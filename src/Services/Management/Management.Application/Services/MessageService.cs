using Management.Application.Constants;
using Management.Application.Contracts.Services;
using Management.Application.Models.DataTransferObjects;
using Management.Application.Options;
using Management.Application.Templates; 
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Shared.Implementations.Communications.Email;
using Shared.Implementations.Tools;

namespace Management.Application.Services;

public class MessageService : IMessageService
{
    private const string ServiceName = "TeamTitan Service";
    private const string InitCompanyMessageSubject = "Confirmation company account"; 

    private readonly IEmailRepository _emailRepository;
    private readonly LinkOptions _linkOptions;
    private readonly EmailOptions _emailOptions;

    public MessageService(IEmailRepository emailRepository, IOptions<EmailOptions> emailOptions, IOptions<LinkOptions> linkOptions)
    {
        _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        _linkOptions = linkOptions?.Value ?? throw new ArgumentNullException(nameof(linkOptions));
        _emailOptions = emailOptions?.Value ?? throw new ArgumentNullException(nameof(emailOptions));
    }

    public async Task SendInitCompanyMessage(string recipient, InitCompanyMessageDto message)
    {  
        var routeUri = new Uri(string.Concat(_linkOptions.Uri, Paths.NewOwnerPath));

        var secretKey = _linkOptions.EncryptionKey;
        var queryParams = new Dictionary<string, string>
        {
            {"Organization", message.CompanyCode.Encrypt(secretKey)},
            {"OwnerCode", message.OwnerCode.Encrypt(secretKey)},
            {"Recipient", message.Recipient.Encrypt(secretKey)}
        };

        var verificationUri = QueryHelpers.AddQueryString(routeUri.ToString(), queryParams!);  
        var template = CompanyTemplates.InitCompanyMessageTemplate(verificationUri);

        await _emailRepository.SendAsync(recipient, template, InitCompanyMessageSubject, ServiceName, _emailOptions);
    }
}