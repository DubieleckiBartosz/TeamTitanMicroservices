using Newtonsoft.Json;

namespace Management.Application.Models.DataTransferObjects;

public record InitCompanyMessageDto
{
    [JsonProperty("OrganizationCode")]
    public string CompanyCode { get; init; }

    [JsonProperty("UserCode")]
    public string OwnerCode { get; init; }

    [JsonProperty("Recipient")]
    public string Recipient { get; init; }

    public InitCompanyMessageDto()
    { 
    }

    public InitCompanyMessageDto(string companyCode, string ownerCode, string recipient)
    {
        CompanyCode = companyCode;
        OwnerCode = ownerCode;
        Recipient = recipient;
    }
}