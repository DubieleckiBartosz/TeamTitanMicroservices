﻿namespace Calculator.Infrastructure.DataAccessObjects.AccountDataAccessObjects;

public class BonusDao
{
    public Guid Id { get; init; }
    public string BonusCode { get; init; }
    public decimal Amount { get; init; }
    public string Creator { get; init; }
    public bool Settled { get; init; }
    public bool Canceled { get; init; }
    public DateTime Created { get; init; }
}