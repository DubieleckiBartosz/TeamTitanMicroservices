using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Snapshots;

public class AccountSnapshot : ISnapshot
{
    public Guid AccountId { get; }
    public int Version { get; private set; }
    public AccountState? State { get; private set; }

    private AccountSnapshot(Guid accountId, int version)
    {
        AccountId = accountId;
        Version = version;
    }
    public static AccountSnapshot Create(Guid accountId, int version)
    {
        return new AccountSnapshot(accountId, version);
    }
    public AccountSnapshot Set(Account account)
    {
        var state = AccountState.CreateAccountState(account);
        State = state;

        return this;
    }
}