﻿namespace Calculator.Domain.Account;

public class Bonus
{
    public string BonusCode { get; private init; }
    public string Creator { get; private init; }
    public decimal Amount { get; private init; }
    public bool Settled { get; private set; }
    public bool Canceled { get; private set; }
    public DateTime Created { get; private init; }

    private Bonus(string creator, string bonusCode, decimal amount)
    {
        BonusCode = bonusCode;
        Amount = amount;
        Settled = false;
        Canceled = false;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    public static Bonus Create(string creator, string bonusCode, decimal amount)
    {
        return new Bonus(creator, bonusCode, amount);
    }

    public Bonus AsSettled()
    {
        Settled = true;
        return this;
    }

    public Bonus AsCanceled()
    {
        Canceled = true;
        return this;
    }
}