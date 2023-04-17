using AutoFixture;
using Calculator.Domain.Account;
using Calculator.Domain.Types;

namespace Calculator.UnitTests.ModelGenerators;

public static class AccountGenerators
{
    public static Account GetAccount(this Fixture fixture, bool? activeAccount = null,
        bool? accountAfterUpdateMethod = null, int? settlementDayMonth = null, bool? withWorkDay = null,
        bool? getWithProducts = null, bool? getWithBonuses = null)
    {
        var accountOwnerCode = fixture.Create<string>();
        var companyCode = fixture.Create<string>();
        var creator = fixture.Create<string>();
        var account = Account.Create(companyCode, accountOwnerCode, creator);
        if (activeAccount != null)
        {
            var modifierUserCode = fixture.Create<string>();

            if (activeAccount.Value)
            {
                account.ActivateAccount(modifierUserCode);
            }
        }

        if (accountAfterUpdateMethod != null && accountAfterUpdateMethod.Value)
        {
            var countingType = CountingType.FixedSalary;
            var workDayHours = fixture.Create<int>();
            settlementDayMonth ??= fixture.Create<int>();
            var expirationDate = fixture.Create<DateTime>();

            account.UpdateAccount(countingType, workDayHours, settlementDayMonth.Value, expirationDate);
        }

        if (withWorkDay != null && withWorkDay.Value)
        {
            account.AddNewWorkDay(DateTime.UtcNow.Date, 0, 0, true, fixture.Create<string>()); 
        }
        
        if (getWithProducts != null && getWithProducts.Value)
        {
            for (var i = 0; i < 10; i++)
            {
                var pieceworkProductId = fixture.Create<Guid>();
                var quantity = fixture.Create<decimal>();
                var currentPrice = fixture.Create<decimal>();
                var date = DateTime.UtcNow;

                account.AddNewPieceProductItem(pieceworkProductId, quantity, currentPrice, date);  
            }
        }

        if (getWithBonuses != null && getWithBonuses.Value)
        {
            for (var i = 0; i < 10; i++)
            { 
                var amount = fixture.Create<decimal>(); 

                account.AddBonus(creator, amount);
            }
        }

        return account;
    } 
}