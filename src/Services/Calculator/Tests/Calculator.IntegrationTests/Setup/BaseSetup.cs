using System.Net.Http.Headers;
using AutoFixture;
using Moq.AutoMock;
using Newtonsoft.Json;
using Shared.Implementations.EventStore;
using Shared.Implementations.Tools;

namespace Calculator.IntegrationTests.Setup;

public abstract class BaseSetup : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory; 
    protected MockServices Mocks;
    protected HttpClient Client;
    protected Fixture Fixture;
    protected AutoMocker Mocker;
    protected BaseSetup(CustomWebApplicationFactory<Program> factory)
    {
        this._factory = factory;
        this.Mocks = _factory.FakeServices();
        this.Mocker = new AutoMocker();
        this.Fixture = new Fixture();
        this.Client = _factory.CreateClient();
    }
    protected JsonSerializerSettings? SerializerSettings() => new JsonSerializerSettings
    {
        ContractResolver = new PrivateResolver(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore
    };
    //protected async Task<IReadOnlyList<StreamState>?> CheckEvents(Guid aggregateId, long? version = null,
    //    DateTime? createdUtc = null)
    //{  
    //}

    //protected async Task ClearEvents()
    //{
    //}

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
}