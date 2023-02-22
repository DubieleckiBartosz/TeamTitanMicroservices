﻿namespace Calculator.Application.ReadModels.BonusReaders;

public class BonusReader
{
    public string Creator { get; }
    public bool Settled { get; private set; }
    public DateTime Created { get; }

    private BonusReader(string creator)
    {
        Settled = false;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    public static BonusReader Create(string creator) => new BonusReader(creator);

    public void AsSettled()
    {
        Settled = true;
    }
}