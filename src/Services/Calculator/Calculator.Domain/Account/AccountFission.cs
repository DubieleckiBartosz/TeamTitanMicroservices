using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Account;

internal class AccountFission
{
    private Account _account;

    internal AccountFission(Account account)
    {
        _account = account;
    }

    private IEvent GetEvent()
    {
        return null;
    }

    private void ActiveAccount(string activatedBy)
    {
        
    }

    private void DeactivateAccount(string deactivatedBy)
    {
    }

    private void UpdateWorkDayHours(int newWorkDayHours)
    {
    }

    private void UpdateHourlyRate(decimal newHourlyRate)
    {
    }

    private void UpdateOvertimeRate(decimal newOvertimeRate)
    {
    }

    private void UpdateCountingType(CountingType newCountingType)
    {
    }

    private void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        var workDay = WorkDay.Create(date, hoursWorked, overtime, isDayOff, createdBy);
    }

    private void AddNewPieceProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice)
    {
    }

}