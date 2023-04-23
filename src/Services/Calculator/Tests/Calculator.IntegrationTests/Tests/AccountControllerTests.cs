using Calculator.Application.Contracts.Repositories;
using Calculator.Application.Features.Account.ViewModels;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Account;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Calculator.IntegrationTests.Generators.Account;
using Calculator.IntegrationTests.Setup;
using Calculator.IntegrationTests.Setup.FakeRepositories;
using Moq;
using Shared.Implementations.Search;

namespace Calculator.IntegrationTests.Tests;

public class AccountControllerTests : BaseSetup
{ 
    private readonly AccountReaderFakeRepository _accountFakeRepository;
    private readonly Mock<IAccountRepository> _accountRepositoryMock; 

    public AccountControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {  
        _accountRepositoryMock = Mocks.AccountRepository;
        _accountFakeRepository = FakeRepositories.GetAccountReaderFakeRepository();
    }

    [Fact]
    public async Task Should_Returns_Accounts_By_Some_Search()
    {
        //Arrange  
        var accounts = _accountFakeRepository.GetAllAccounts();
        var searchResponse = ResponseSearchList<AccountReader>.Create(accounts.ToList(), accounts.Count);

        _accountRepositoryMock.Setup(_ => _.GetAccountsBySearchAsync(
                It.IsAny<Guid?>(), It.IsAny<CountingType?>(), 
                It.IsAny<AccountStatus?>(), It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<decimal?>(),
                It.IsAny<decimal?>(), It.IsAny<int?>(), 
                It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), 
                It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(searchResponse);


        var request = Fixture.GetAccountsSearchParameters();

        //Act
        var response = await ClientCall(request, HttpMethod.Post, Urls.SearchAccountsPath);
        var responseData = await ReadFromResponse<AccountSearchViewModel>(response);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(accounts.Count, responseData!.TotalCount);
    }

    [Fact]
    public async Task Should_Add_BonusTo_Account()
    {
        //Arrange
        var eventReader = _accountFakeRepository.GetFirstAccountAfterDataUpdateWithEvents();
        var account = eventReader!.Reader;
        var streamList = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(streamList); 
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
        var streamList = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(streamList);
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
        var streamList = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(streamList);
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
        var streamList = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(streamList);
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
        var streamList = eventReader.ToStreamList<Account>(account.Id);

        this.SetupStore(streamList);
        _accountRepositoryMock.Setup(_ => _.GetAccountByIdAsync(account!.Id)).ReturnsAsync(account);

        var request = Fixture.GetDeactivateAccountParameters(account!.Id);

        //Act
        var response = await ClientCall(request, HttpMethod.Put, Urls.DeactivateAccountPath);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
        _accountRepositoryMock.Verify(v => v.UpdateStatusToDeactivateAsync(It.IsAny<AccountReader>()), Times.Once);
    }
}