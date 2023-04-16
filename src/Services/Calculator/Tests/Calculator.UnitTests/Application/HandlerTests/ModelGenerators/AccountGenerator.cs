using AutoFixture;
using Calculator.Domain.Account;
using Calculator.Domain.Types;

namespace Calculator.UnitTests.Application.HandlerTests.ModelGenerators;

public static class AccountGenerator
{
    public static Account GenerateAccount(this Fixture fixture)
    {
        var accountOwnerCode = fixture.Create<string>();
        var companyCode = fixture.Create<string>();
        var creator = fixture.Create<string>();
        var account = Account.Create(companyCode, accountOwnerCode, creator);

        return account;
    }
    
    public static Account GenerateAccountWithBaseData(this Fixture fixture, 
        CountingType? countingType = null, int? workDayHours = null,
        int? settlementDayMonth = null, DateTime? expirationDate = null)
    {
        var rnd = new Random();
        var account = fixture.GenerateAccount();

        countingType ??= fixture.Create<CountingType>();
        workDayHours ??= rnd.Next(4, 12);
        settlementDayMonth ??= rnd.Next(1, 28);
        expirationDate ??= fixture.Create<DateTime>();

        account.UpdateAccount(countingType.Value, workDayHours.Value, settlementDayMonth.Value, expirationDate);
        return account;
    }
}