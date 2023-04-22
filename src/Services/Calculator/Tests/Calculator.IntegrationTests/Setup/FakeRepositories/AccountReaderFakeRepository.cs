using AutoFixture;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.IntegrationTests.Generators.Account;

namespace Calculator.IntegrationTests.Setup.FakeRepositories;

public class AccountReaderFakeRepository
{
    private readonly Dictionary<AccountKey, List<EventReader<AccountReader>>> _accountsDictionary;
    private readonly List<AccountReader> _accounts; 

    public enum AccountKey
    {
        Initiated = 1,
        Activated = 2,
        Deactivated = 3,
        AfterDataUpdate = 4,
        AfterFinancialDataUpdate = 5,
        BonusAdded = 6,
        Settled = 7
    }
     
    private readonly Fixture _fixture;

    public AccountReaderFakeRepository(Fixture fixture)
    { 
        _fixture = fixture;
        _accountsDictionary = new Dictionary<AccountKey, List<EventReader<AccountReader>>>
        {
            {AccountKey.Initiated, GetInitiatedReaderAccounts()},
            {AccountKey.Activated, GetActivatedAccounts()},
            {AccountKey.Deactivated, GetDeactivatedAccounts()},
            {AccountKey.AfterDataUpdate, GetAfterDataUpdateAccounts()},
            {AccountKey.AfterFinancialDataUpdate, GetWithFinancialDataAccounts()},
            {AccountKey.BonusAdded, GetWithBonusAccounts()},
            {AccountKey.Settled, GetWithSettlementAccounts()}
        };

        _accounts = _accountsDictionary.SelectMany(_ => _.Value.Select(_ => _.Reader)).ToList();
    }
    
    //Get first or null
    public EventReader<AccountReader>? GetFirstInitiatedAccountWithEvents() => _accountsDictionary[AccountKey.Initiated][0];
    public EventReader<AccountReader>? GetFirstActivatedAccountWithEvents() => _accountsDictionary[AccountKey.Activated][0];
    public EventReader<AccountReader>? GetFirstDeactivatedAccountWithEvents() => _accountsDictionary[AccountKey.Deactivated][0];
    public EventReader<AccountReader>? GetFirstAccountAfterDataUpdateWithEvents() => _accountsDictionary[AccountKey.AfterDataUpdate][0];
    public EventReader<AccountReader>? GetFirstAccountAfterSettlementWithEvents() => _accountsDictionary[AccountKey.Settled][0];
    public EventReader<AccountReader>? GetFirstAccountAfterFinancialDataUpdateWithEvents() => _accountsDictionary[AccountKey.AfterFinancialDataUpdate][0]; 
    public EventReader<AccountReader>? GetFirstAccountWithBonusWithEvents() => _accountsDictionary[AccountKey.BonusAdded][0];

    //Find by condition or null
    public EventReader<AccountReader>? GetOldDeactivatedAccountWithEvents() =>
        _accountsDictionary[AccountKey.Deactivated].FirstOrDefault(_ => _.Reader.Settlements.Any());


    public IReadOnlyList<AccountReader> GetAllAccounts() => _accounts.AsReadOnly(); 

    private List<EventReader<AccountReader>> GetInitiatedReaderAccounts(int count = 1)
    { 
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var initiatedEvent = AccountEventGenerator.GetNewAccountInitiatedEvent();
            var account = AccountReaderGenerator.GetAccountReaderInitiated(initiatedEvent);
            var events = new List<IEvent>() { initiatedEvent };

            response.Add(new EventReader<AccountReader>(account, events));
        }

        return response;
    }

    private List<EventReader<AccountReader>> GetAfterDataUpdateAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetInitiatedReaderAccounts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;
                var accountDataUpdatedEvent = AccountEventGenerator.GetAccountDataUpdatedEvent(_fixture, reader.Id); 
                var account = AccountReaderGenerator.GetAccountReaderCompletedData(accountDataUpdatedEvent, reader);
                eventReader.Events.Add(accountDataUpdatedEvent);

                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }

    private List<EventReader<AccountReader>> GetActivatedAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetAfterDataUpdateAccounts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;
                var accountActivatedEvent = AccountEventGenerator.GetAccountActivatedEvent(reader.Id);
                var account = AccountReaderGenerator.GetActivatedAccountReader(accountActivatedEvent, reader);

                eventReader.Events.Add(accountActivatedEvent);
                 
                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }

    private List<EventReader<AccountReader>> GetDeactivatedAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetActivatedAccounts(count);
            var oldEventReaders = GetWithSettlementAccounts(count);

            eventReaders.AddRange(oldEventReaders!);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;

                var accountDeactivatedEvent = AccountEventGenerator.GetAccountDeactivatedEvent(reader.Id);
                var account = AccountReaderGenerator.GetDeactivatedAccountReader(accountDeactivatedEvent, reader);
                eventReader.Events.Add(accountDeactivatedEvent);

                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }
    
    private List<EventReader<AccountReader>> GetWithFinancialDataAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetAfterDataUpdateAccounts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;

                var financialDataUpdatedEvent = AccountEventGenerator.GetFinancialDataUpdatedEvent(_fixture, reader.Id);
                var account = AccountReaderGenerator.GetAccountReaderAfterUpdateFinancialData(financialDataUpdatedEvent, reader);
                eventReader.Events.Add(financialDataUpdatedEvent);

                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }
    
    private List<EventReader<AccountReader>> GetWithBonusAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetWithFinancialDataAccounts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;

                var bonusAddedEvent = AccountEventGenerator.GetBonusAddedEvent(_fixture);
                var account = AccountReaderGenerator.GetAccountReaderWithBonus(bonusAddedEvent, reader);
                eventReader.Events.Add(bonusAddedEvent);

                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }

    private List<EventReader<AccountReader>> GetWithSettlementAccounts(int count = 1)
    {
        var response = new List<EventReader<AccountReader>>();

        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetWithFinancialDataAccounts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;

                var accountSettledEvent = AccountEventGenerator.GetAccountSettledEvent(_fixture, reader.Id);
                var account = AccountReaderGenerator.GetAccountWithSettlement(accountSettledEvent, reader);
                eventReader.Events.Add(accountSettledEvent);

                response.Add(new EventReader<AccountReader>(account, eventReader.Events));
            }
        }

        return response;
    }
}