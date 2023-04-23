using Calculator.IntegrationTests.Common;
using Calculator.IntegrationTests.Setup.FakeRepositories;
using Microsoft.AspNetCore.Authorization.Policy;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Calculator.IntegrationTests.Setup;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    private MockServices? _servicesMock;
    private FakeDataRepositories? _fakeDataRepositories;
    public MockServices FakeServices()
    {
        _servicesMock ??= new MockServices();

        return _servicesMock;
    }
    public FakeDataRepositories FakeRepositories()
    {
        _fakeDataRepositories ??= new FakeDataRepositories();

        return _fakeDataRepositories;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddHangfire(c => c.UseMemoryStorage()); 
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(_ => _.Filters.Add(new FakeUserFilter()));

            MockServices.RegisterMockServices(services: ref services, FakeServices().GetMocks());
        });
    }
     
}