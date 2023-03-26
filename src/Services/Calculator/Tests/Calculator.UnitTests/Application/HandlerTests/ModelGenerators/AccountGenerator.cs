using AutoFixture;
using Calculator.Domain.Account;

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
}