namespace Management.Application.Models.DataTransferObjects;

public class CompanyCodesDto
{
    public string CompanyCode { get; init; }
    public string OwnerCode { get; init; }

    public CompanyCodesDto(string companyCode, string ownerCode)
    {
        CompanyCode = companyCode;
        OwnerCode = ownerCode;
    }
}