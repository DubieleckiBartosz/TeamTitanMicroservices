using AutoFixture;
using Calculator.Domain.Types;
using Calculator.UnitTests.ModelGenerators;

using Shared.Domain.DomainExceptions;

namespace Calculator.UnitTests.Domain.Tests;

public class AccountTests : DomainBaseTests
{
    private readonly string _modifierUserCode;
    private readonly int _newWorkDayHours;
    public AccountTests()
    {
        _modifierUserCode = Fixture.Create<string>();
        _newWorkDayHours = Fixture.Create<int>();
    }

    [Fact]
    public void Should_Create_Account()
    { 
        //Arrange and Act
        var account = Fixture.GetAccount();

        //Assert
        Assert.NotNull(account);
    }

    [Fact]
    public void Should_Update_Account()
    {
        //Arrange
        var account = Fixture.GetAccount();
        var countingType = Fixture.Create<CountingType>();
        var workDayHours = account.Details.WorkDayHours + 1;
        var settlementDayMonth = Fixture.Create<int>();
        var expirationDate = Fixture.Create<DateTime>();

        var workDayHoursBefore = account.Details.WorkDayHours; 

        //Act
        account.UpdateAccount(countingType, workDayHours, settlementDayMonth, expirationDate);
        var workDayHoursAfter = account.Details.WorkDayHours;

        Assert.True(workDayHoursBefore != workDayHoursAfter);
    }

    [Fact]
    public void Should_Activate_Account()
    {
        //Arrange
        var account = Fixture.GetAccount(); 

        var activationStatusBefore = account.IsActive;

        //Act
        account.ActivateAccount(_modifierUserCode);

        //Assert
        var activationStatusAfter = account.IsActive;

        Assert.False(activationStatusBefore);
        Assert.True(activationStatusAfter);
    }

    [Fact]
    public void Should_Throw_Exception_On_Account_Activation_When_It_Is_Already_Activated()
    {
        //Arrange
        var account = Fixture.GetAccount(activeAccount: true);

        //Act and Assert
        var result = Assert.Throws<BusinessException>(() => account.ActivateAccount(_modifierUserCode));

        Assert.Equal("Bad current status".ToLower(), result.Title.ToLower());
    }
     
    [Fact]
    public void Should_Throw_Exception_On_Account_Deactivation_When_It_Is_Already_Deactivated()
    {
        //Arrange
        var account = Fixture.GetAccount();

        //Act and Assert
        var result = Assert.Throws<BusinessException>(() => account.DeactivateAccount(_modifierUserCode));

        Assert.Equal("Bad current status".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Deactivate_Account()
    {
        //Arrange
        var account = Fixture.GetAccount(activeAccount: true);

        var activationStatusBefore = account.IsActive;

        //Act   
        account.DeactivateAccount(_modifierUserCode);

        //Assert
        var activationStatusAfter = account.IsActive;

        Assert.True(activationStatusBefore);
        Assert.False(activationStatusAfter);
    }

    [Fact]
    public void Should_Throw_Exception_When_Account_Is_Not_Active()
    {
        //Arrange
        var account = Fixture.GetAccount(); 

        //Act and Assert
        var result = Assert.Throws<BusinessException>(() => account.UpdateWorkDayHours(_newWorkDayHours));

        Assert.Equal("Incorrect account status".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Update_Work_Day_Hours()
    {
        //Arrange
        var account = Fixture.GetAccount(activeAccount: true, accountAfterUpdateMethod: true);
        var newValue = account.Details.WorkDayHours + 1;

        //Act
        account.UpdateWorkDayHours(newValue);

        //Assert
        var workDayHoursAfter = account.Details.WorkDayHours;

        Assert.Equal(newValue, workDayHoursAfter);
    }

    [Fact]
    public void Should_Update_Counting_Type()
    {
        //Arrange
        var account = Fixture.GetAccount(activeAccount: true, accountAfterUpdateMethod: true);

        var countingTypeBefore = account.Details.CountingType;

        //Act
        account.UpdateCountingType(CountingType.ForAnHour);

        //Assert
        var countingTypeAfter = account.Details.CountingType;

        Assert.True(countingTypeAfter != countingTypeBefore);

    }

    [Fact]
    public void Should_Throw_Exception_When_Hours_Worked_Day_Are_Greater_Than_Agreed_Hours()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true);

        //Act and assert
        var result = Assert.Throws<BusinessException>(() =>
            account.AddNewWorkDay(DateTime.UtcNow.Date, account.Details.WorkDayHours + 1, 0, false,
                Fixture.Create<string>()));

        Assert.Equal("Incorrect number of hours".ToLower(), result.Title.ToLower());
    }

    [Theory]
    [MemberData(nameof(GetDateOutOfRange))]
    public void Should_Throw_Exception_When_Date_Out_Of_Range(DateTime date)
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15);

        //Act and assert
        var result = Assert.Throws<BusinessException>(() =>
            account.AddNewWorkDay(date, account.Details.WorkDayHours, 0, false,
                Fixture.Create<string>()));

        Assert.Equal("Incorrect date".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Throw_Exception_When_Dates_Overlap()
    { 
        //Arrange
        var account = Fixture.GetAccount(true, true, 15, withWorkDay: true);

        //Act and assert
        var result = Assert.Throws<BusinessException>(() =>
            account.AddNewWorkDay(DateTime.UtcNow.Date, 0, 0, true,
                Fixture.Create<string>()));

        Assert.Equal("Dates overlap".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Add_New_Work_Day()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15);

        var workDaysCountBefore = account.WorkDays.Count;

        //Act
        account.AddNewWorkDay(DateTime.UtcNow.Date, 0, 0, true,
            Fixture.Create<string>());

        //Assert
        var workDaysCountAfter = account.WorkDays.Count;

        Assert.Equal(workDaysCountBefore + 1, workDaysCountAfter);
    }

    [Fact]
    public void Should_Add_Piece_Product_Item_To_Account()
    {       
        //Arrange
        var account = Fixture.GetAccount(true, true, 15);
        var pieceworkProductId = Fixture.Create<Guid>();
        var quantity = Fixture.Create<decimal>();
        var currentPrice = Fixture.Create<decimal>();
        var date = DateTime.UtcNow;

        var pieceProductItemsCountBefore = account.ProductItems.Count;

        //Act
        account.AddNewPieceProductItem(pieceworkProductId, quantity, currentPrice, date);

        //Assert
        var pieceProductItemsCountAfter = account.ProductItems.Count;

        Assert.Equal(pieceProductItemsCountBefore + 1, pieceProductItemsCountAfter);
    }

    [Fact]
    public void Should_Add_Nww_Settlement_To_Account()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15, getWithProducts: true, getWithBonuses: true);

        var settlementsBefore = account.Settlements.Count;
        var productsNotConsideredBefore = account.ProductItems.All(_ => _.IsConsidered == false);
        var bonusesNotSettledBefore = account.Bonuses.All(_ => _.Settled == false);
        var balanceBefore = account.Details.Balance;

        //Act
        account.AccountSettlement();

        //Assert
        var balanceAfter = account.Details.Balance;
        var settlementsAfter = account.Settlements.Count;

        Assert.True(productsNotConsideredBefore);
        Assert.True(bonusesNotSettledBefore);
        Assert.True(account.ProductItems.All(_ => _.IsConsidered));
        Assert.True(account.Bonuses.All(_ => _.Settled));
        Assert.True(balanceBefore > 0);
        Assert.Equal(0, balanceAfter);
        Assert.Equal(settlementsBefore + 1, settlementsAfter); 

    }

    [Fact]
    public void Should_Add_New_Bonus()
    {
        //Act
        var account = Fixture.GetAccount(true, true, 15);
        var amount = Fixture.Create<decimal>();
        var creator = Fixture.Create<string>();
        var bonusesCountBefore = account.Bonuses.Count;

        //Act
        account.AddBonus(creator, amount);

        //Assert
        var bonusesCountAfter = account.Bonuses.Count;

        Assert.Equal(bonusesCountBefore + 1, bonusesCountAfter);
    }

    [Fact]
    public void Should_Throw_Exception_When_List_Of_Bonuses_Is_Null()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15);
        
        //Act and Assert
        var result = Assert.Throws<BusinessException>(() =>
            account.CancelBonus(Fixture.Create<string>()));

        Assert.Equal("List is empty".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Throw_Exception_When_Bonus_Not_Found()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15, getWithBonuses: true);
 
        //Act and Assert
        var result = Assert.Throws<BusinessException>(() =>
            account.CancelBonus(Fixture.Create<string>()));

        Assert.Equal("Bonus is NULL".ToLower(), result.Title.ToLower());
    }

    [Fact]
    public void Should_Cancel_Bonus()
    {
        //Arrange
        var account = Fixture.GetAccount(true, true, 15, getWithBonuses: true);
        var bonusToCancel = account.Bonuses.Last().BonusCode;
        var allBonusesNotCanceled = account.Bonuses.All(_ => !_.Canceled);

        //Act 
        account.CancelBonus(bonusToCancel);
        var someBonusCanceled = account.Bonuses.Any(_ => _.Canceled);
        
        //Assert 
        Assert.True(allBonusesNotCanceled);
        Assert.True(someBonusCanceled);
    }

    public static IEnumerable<object[]> GetDateOutOfRange()
    {
        var now = DateTime.UtcNow;

        var tooEarlyDate = now.AddMonths(-1); 
        var tooLateDate = now.AddMonths(1); 

        var tooEarly = new DateTime(tooEarlyDate.Year, tooEarlyDate.Month, 14);
        var tooLate = new DateTime(tooLateDate.Year, tooLateDate.Month, 15);

        yield return new object[]
        {
            tooEarly
        };

        yield return new object[]
        {
            tooLate
        }; 
    }
 }