using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Account.Events;

namespace Calculator.IntegrationTests.Generators.Account;

public class AccountReaderGenerator
{
    public static AccountReader GetAccountReaderInitiated(NewAccountInitiated @event)
    {
        return AccountReader.Create(@event);
    }

    public static AccountReader GetAccountReaderCompletedData(AccountDataUpdated @event, AccountReader accountReader)
    {
        return accountReader.DataCompleted(@event);
    }

    public static AccountReader GetActivatedAccountReader(AccountActivated @event, AccountReader accountReader)
    {
        return accountReader.AccountActivated(@event);
    }

    public static AccountReader GetDeactivatedAccountReader(AccountDeactivated @event, AccountReader accountReader)
    {
        return accountReader.AccountDeactivated(@event);
    }

    public static AccountReader GetAccountReaderAfterUpdateFinancialData(FinancialDataUpdated @event,
        AccountReader accountReader)
    {
        return accountReader.AssignFinancialData(@event);
    }

    public static AccountReader GetAccountReaderWithBonus(BonusAdded @event, AccountReader accountReader)
    {
        accountReader.BonusToAccountAdded(@event);
        return accountReader;
    }

    public static AccountReader GetAccountWithSettlement(AccountSettled @event, AccountReader accountReader)
    {
        accountReader.Settled(@event);
        return accountReader;
    }
}