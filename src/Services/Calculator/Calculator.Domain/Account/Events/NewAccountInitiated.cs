using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record NewAccountInitiated(string DepartmentCode, string AccountCode, string CreatedBy, Guid AccountId) : IEvent
{
    public static NewAccountInitiated Create(string departmentCode, string accountOwnerCode, string createdBy, Guid accountId)
    {
        return new NewAccountInitiated(departmentCode, accountOwnerCode, createdBy, accountId);
    }
}