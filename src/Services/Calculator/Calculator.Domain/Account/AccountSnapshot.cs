using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account;

public class AccountSnapshot : ISnapshot
{
    public Guid AccountId { get; }
    public AccountState? State { get; private set; }

    private AccountSnapshot(Guid accountId)
    {
        AccountId = accountId; 
    }
    public static AccountSnapshot Create(Guid accountId)
    {
        return new AccountSnapshot(accountId);
    }
    public AccountSnapshot Set(Account account)
    {
        var state = AccountState.CreateAccountState(account);
        State = state;

        return this;
    }
}