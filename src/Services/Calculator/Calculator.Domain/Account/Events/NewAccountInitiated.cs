using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record NewAccountInitiated(string DepartmentCode, string AccountExternal, string CreatedBy, Guid AccountId) : IEvent
{
    public static NewAccountInitiated Create(string departmentCode, string accountOwnerExternalId, string createdBy, Guid accountId)
    {
        return new NewAccountInitiated(departmentCode, accountOwnerExternalId, createdBy, accountId);
    }
}