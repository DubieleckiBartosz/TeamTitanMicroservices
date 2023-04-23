using AutoFixture;
using Calculator.Application.Parameters.AccountParameters;

namespace Calculator.IntegrationTests.Generators.Account;

public static class AccountRequestsGenerator
{ 
    public static AddBonusToAccountParameters GetAddBonusToAccountParameters(this Fixture fixture,
        Guid? accountId = null)
    {
        return fixture.Build<AddBonusToAccountParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid())
            .Create();
    }

    public static CancelBonusAccountParameters GetCancelBonusAccountParameters(this Fixture fixture, string bonusCode,
        Guid? accountId = null)
    {
        return fixture.Build<CancelBonusAccountParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid())
            .With(w => w.BonusCode, bonusCode)
            .Create();
    }

    public static AddPieceProductParameters GetAddPieceProductParameters(this Fixture fixture,
        Guid? accountId = null)
    {
        return fixture.Build<AddPieceProductParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid())
            .With(w => w.Date, DateTime.UtcNow)
            .Create();
    }

    public static AddWorkDayParameters GetAddWorkDayParameters(this Fixture fixture,
        Guid? accountId = null)
    {
        var rnd = new Random();
        return fixture.Build<AddWorkDayParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid())
            .With(w => w.Date, DateTime.UtcNow)
            .With(w => w.IsDayOff, false)
            .With(_ => _.HoursWorked, rnd.Next(1, 5))
            .With(_ => _.Overtime, rnd.Next(1, 5))
            .Create();
    }
    public static ActivateAccountParameters GetActivateAccountParameters(this Fixture fixture,
        Guid? accountId = null)
    { 
        return fixture.Build<ActivateAccountParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid()) 
            .Create();
    }
    
    public static DeactivateAccountParameters GetDeactivateAccountParameters(this Fixture fixture,
        Guid? accountId = null)
    { 
        return fixture.Build<DeactivateAccountParameters>()
            .With(w => w.AccountId, accountId ?? Guid.NewGuid()) 
            .Create();
    }
}