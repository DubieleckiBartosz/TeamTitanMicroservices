using AutoFixture;
using Calculator.Domain.Account.Events;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Calculator.IntegrationTests.Settings;

namespace Calculator.IntegrationTests.Generators.Account;

public class AccountEventGenerator
{
    public static NewAccountInitiated GetNewAccountInitiatedEvent()
    {
        var accountOwnerCode = GlobalSettings.SomeReceiverCode;
        var companyCode = GlobalSettings.OrganizationCode;
        var creator = GlobalSettings.UserVerificationCode;

        return NewAccountInitiated.Create(companyCode, accountOwnerCode, creator, Guid.NewGuid());
    }

    public static BonusAdded GetBonusAddedEvent(Fixture fixture)
    {
        var bonusAmount = fixture.Create<decimal>();
        var creator = fixture.Create<string>();
        var accountId = fixture.Create<Guid>();
        var bonusCode = fixture.Create<string>();

        return BonusAdded.Create(bonusAmount, creator, accountId, bonusCode);

    }

    public static AccountDataUpdated GetAccountDataUpdatedEvent(Fixture fixture, Guid? accountId = null)
    {
        var rnd = new Random();
        var countingType = fixture.Create<CountingType>();
        var workDayHours = rnd.Next(6, 8);
        var settlementDayMonth = rnd.Next(1, 28);
        var expirationDate = DateTime.UtcNow.AddDays(rnd.Next(30, 90));

        return AccountDataUpdated.Create(countingType, AccountStatus.New, workDayHours, settlementDayMonth, accountId ?? Guid.NewGuid(),
            expirationDate); 
    }
    public static FinancialDataUpdated GetFinancialDataUpdatedEvent(Fixture fixture, Guid? accountId = null)
    {
        var paymentAmount = fixture.Create<decimal>(); 
        var overtimeRate = fixture.Create<decimal>(); 
        var hourlyRate = fixture.Create<decimal>();

        return FinancialDataUpdated.Create(paymentAmount, overtimeRate, hourlyRate, accountId ?? Guid.NewGuid());
    }

    public static AccountActivated GetAccountActivatedEvent(Guid? accountId = null)
    {
        return AccountActivated.Create(GlobalSettings.UserVerificationCode, accountId ?? Guid.NewGuid()); 
    }

    public static AccountDeactivated GetAccountDeactivatedEvent(Guid? accountId = null)
    {
        return AccountDeactivated.Create(GlobalSettings.UserVerificationCode, accountId ?? Guid.NewGuid()); 
    }

    public static AccountSettled GetAccountSettledEvent(Fixture fixture, Guid? accountId = null)
    {
        var balance = fixture.Create<decimal>();
        var from = DateTime.Today.AddMonths(-1);
        var to = DateTime.Today;
        return AccountSettled.Create(accountId!.Value, balance, from, to);
    }
}