using Calculator.IntegrationTests.Common;
using Microsoft.AspNetCore.Authorization.Policy;
using Shared.Implementations.EventStore;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;

namespace Calculator.IntegrationTests.Setup;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    private MockServices? _servicesMock;

    public MockServices FakeServices()
    {
        _servicesMock ??= new MockServices();

        return _servicesMock;
    } 

    private const string? ConnectionEventStore = "mongodb://rootuser:rootpass@localhost:27018";
    private const string? StoreDatabase = "TeamTitanTestDatabase"; 

    private MongoClient? _eventStoreConnection; 
    public MongoClient? GetEventStore() => _eventStoreConnection ?? OpenEventStoreConnection();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddHangfire(c => c.UseMemoryStorage());

            services.Configure<StoreOptions>(opts =>
            {
                opts.ConnectionString = ConnectionEventStore!;
                opts.DatabaseName = StoreDatabase!;
            }); 

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(_ => _.Filters.Add(new FakeUserFilter()));

            MockServices.RegisterMockServices(services: ref services, FakeServices().GetMocks());
        });
    }

   
    private MongoClient? OpenEventStoreConnection()
    {
        _eventStoreConnection = new MongoClient(ConnectionEventStore);
        return _eventStoreConnection;
    } 
}