using System.Net.Http.Headers;
using AutoFixture;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using Shared.Implementations.EventStore;
using Shared.Implementations.Snapshot;
using Shared.Implementations.Tools;

namespace Calculator.IntegrationTests.Setup;

public abstract class BaseSetup : IClassFixture<CustomWebApplicationFactory<Program>>
{ 
    protected MockServices Mocks;
    protected HttpClient Client;
    protected Fixture Fixture;
    protected AutoMocker Mocker;
    protected BaseSetup(CustomWebApplicationFactory<Program> factory)
    { 
        this.Mocks = factory.FakeServices();
        this.Mocker = new AutoMocker();
        this.Fixture = new Fixture();
        this.Client = factory.CreateClient();
    }

    protected JsonSerializerSettings? SerializerSettings() => new JsonSerializerSettings
    {
        ContractResolver = new PrivateResolver(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore
    }; 

    protected async Task<HttpResponseMessage> ClientCall<TRequest>(TRequest? obj, HttpMethod methodType,
        string requestUri)
    {
        var request = new HttpRequestMessage(methodType, requestUri);
        if (obj != null)
        {
            var serializeObject = JsonConvert.SerializeObject(obj);
            request.Content = new StringContent(serializeObject);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        return await this.Client.SendAsync(request);
    }


    protected async Task<TResponse?> ReadFromResponse<TResponse>(HttpResponseMessage response)
    {
        var contentString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(contentString, this.SerializerSettings());
    }

    protected void SetupStore(List<StreamState> streamStates)
    {
        Mocks.SnapshotStore.Setup(_ => _.GetLastSnapshotAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        Mocks.Store.Setup(_ =>
                _.GetEventsAsync(It.IsAny<Guid>(), null, It.IsAny<long?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(streamStates);

        //it is not necessary, but for readability we do setup
        Mocks.Store.Setup(_ => _.AddAsync(It.IsAny<StreamState>(), It.IsAny<long>()));
    }

    protected void SetupSnapShotStore(SnapshotState snapshotStates)
    {
        Mocks.SnapshotStore.Setup(_ => _.GetLastSnapshotAsync(It.IsAny<Guid>())).ReturnsAsync(snapshotStates);
    }
}