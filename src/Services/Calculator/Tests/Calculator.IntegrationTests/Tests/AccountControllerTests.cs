using Calculator.Application.Contracts.Repositories;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Account;
using Calculator.Domain.Account.Snapshots;
using Calculator.IntegrationTests.Generators.Account;
using Calculator.IntegrationTests.Setup;
using Calculator.IntegrationTests.Setup.FakeRepositories;
using Moq;

namespace Calculator.IntegrationTests.Tests;

public class AccountControllerTests : BaseSetup
{ 
    private readonly AccountReaderFakeRepository _accountFakeRepository;
    private readonly Mock<IAccountRepository> _accountRepositoryMock; 

    public AccountControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {  
        _accountRepositoryMock = Mocks.AccountRepository;
        _accountFakeRepository = new AccountReaderFakeRepository(Fixture);
    }

    [Fact]
    public async Task Should_Add_BonusTo_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountAfterDataUpdateWithEvents();
        var account = eventReader!.Reader;
        var events = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(events); 
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdWithBonusesAsync(account!.Id)).ReturnsAsync(account);
        //it is not necessary, but for readability we do setup
        _accountRepositoryMock.Setup(_ => _.AddBonusAsync(It.IsAny<AccountReader>()));

        var request = Fixture.GetAddBonusToAccountParameters(account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.AddBonusToAccountPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.AddBonusAsync(It.IsAny<AccountReader>()), Times.Once); 
    }

    [Fact]
    public async Task Should_Cancel_Bonus_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountWithBonusWithEvents();
        var account = eventReader!.Reader;
        var events = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(events);
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdWithBonusesAsync(account!.Id)).ReturnsAsync(account);
        //it is not necessary, but for readability we do setup
        _accountRepositoryMock.Setup(_ =>
            _.UpdateBonusAccountAsync(It.IsAny<BonusReader>(), It.IsAny<AccountReader>()));

        var request = Fixture.GetCancelBonusAccountParameters(account.Bonuses[0].BonusCode, account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.CancelBonusPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(
            v => v.UpdateBonusAccountAsync(It.IsAny<BonusReader>(), It.IsAny<AccountReader>()), Times.Once);
    }

    [Fact]
    public async Task Should_Add_Piece_Product_To_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountAfterDataUpdateWithEvents();
        var account = eventReader!.Reader;
        var events = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(events);
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdAsync(account!.Id)).ReturnsAsync(account);

        var request = Fixture.GetAddPieceProductParameters(account!.Id);
        
        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.AddPieceProductPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.AddProductItemAsync(It.IsAny<AccountReader>()), Times.Once);
    }

    [Fact]
    public async Task Should_Add_Work_Day_To_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountAfterDataUpdateWithEvents();
        var account = eventReader!.Reader;
        var events = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(events);
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdAsync(account!.Id)).ReturnsAsync(account);

        var request = Fixture.GetAddWorkDayParameters(account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.AddWorkDayPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.AddNewWorkDayAsync(It.IsAny<AccountReader>()), Times.Once);
    }


    [Fact]
    public async Task Should_Activate_Old_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetOldDeactivatedAccountWithEvents();
        var account = eventReader!.Reader;

        var snapshot = eventReader.ToSnapshot<AccountSnapshot, Account>(account.Id);
        SetupSnapShotStore(snapshot);

        _accountRepositoryMock.Setup(_ => _.GetAccountByIdAsync(account!.Id)).ReturnsAsync(account);

        var request = Fixture.GetActivateAccountParameters(account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.ActivateAccountPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.UpdateStatusToActiveAsync(It.IsAny<AccountReader>()), Times.Once);
    }

    [Fact]
    public async Task Should_Deactivate_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountAfterDataUpdateWithEvents();
        var account = eventReader!.Reader;
        var events = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(events);
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdAsync(account!.Id)).ReturnsAsync(account);

        var request = Fixture.GetDeactivateAccountParameters(account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.DeactivateAccountPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.UpdateStatusToDeactivateAsync(It.IsAny<AccountReader>()), Times.Once);
    }
}