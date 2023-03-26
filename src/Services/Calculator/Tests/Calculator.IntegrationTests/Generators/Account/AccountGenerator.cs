using AutoFixture;
using Calculator.IntegrationTests.Settings;

namespace Calculator.IntegrationTests.Generators.Account;

public static class AccountGenerator
{
    public static Domain.Account.Account GetInitiatedAccount()
    {
        var accountOwnerCode = GlobalSettings.SomeReceiverCode;
        var companyCode = GlobalSettings.OrganizationCode;
        var creator = GlobalSettings.UserVerificationCode;

        return Domain.Account.Account.Create(companyCode, accountOwnerCode, creator);
    }

    public static Domain.Account.Account GetActivatedAccount()
    {
        var account = GetInitiatedAccount();
        account.ActiveAccount(GlobalSettings.UserVerificationCode);
        return account;
    }

    public static Domain.Account.Account GetDeactivatedAccount()
    {
        var account = GetInitiatedAccount();
        account.DeactivateAccount(GlobalSettings.UserVerificationCode);
        return account;
    }
}