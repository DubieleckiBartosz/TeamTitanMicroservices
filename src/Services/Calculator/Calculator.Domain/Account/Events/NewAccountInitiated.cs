using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record NewAccountInitiated(string CompanyCode, string AccountOwnerCode, string CreatedBy, Guid AccountId) : IEvent
{
    public static NewAccountInitiated Create(string companyCode, string accountOwnerCode, string createdBy, Guid accountId)
    {
        return new NewAccountInitiated(companyCode, accountOwnerCode, createdBy, accountId);
    }
}