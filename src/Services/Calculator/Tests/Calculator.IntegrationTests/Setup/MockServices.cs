using Moq;
using Shared.Implementations.RabbitMQ;
using Shared.Implementations.Snapshot;
using System.Reflection;
using Calculator.Application.Contracts.Repositories;
using Shared.Implementations.Background;
using Shared.Implementations.EventStore;

namespace Calculator.IntegrationTests.Setup;

public class MockServices
{
    public Mock<IRabbitBase> RabbitBaseMock { get; }
    public Mock<IRabbitEventListener> RabbitEventListenerMock { get; }
    public Mock<ISnapshotStore> SnapshotStore { get; }
    public Mock<IStore> Store { get; } 
    public Mock<IAccountRepository> AccountRepository { get; }
    public Mock<IProductRepository> ProductRepositoryMock { get; }
    public Mock<IJobService> JobServiceMock { get; }

    public MockServices()
    {
        RabbitBaseMock = new Mock<IRabbitBase>();
        RabbitEventListenerMock = new Mock<IRabbitEventListener>();
        SnapshotStore = new Mock<ISnapshotStore>();
        Store = new Mock<IStore>(); 
        AccountRepository = new Mock<IAccountRepository>();
        ProductRepositoryMock = new Mock<IProductRepository>();
        JobServiceMock = new Mock<IJobService>();
    }
    public (Type underlyingType, object Object)[] GetMocks()
    {
        return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(_ =>
        {
            var underlyingType = _.PropertyType.GetGenericArguments()[0];
            var value = _.GetValue(this) as Mock;

            return (underlyingType, value?.Object!);
        }).ToArray();
    }

    public static void RegisterMockServices(ref IServiceCollection services, (Type underlyingType, object Object)[] mocks)
    {
        foreach (var (interfaceType, serviceMock) in mocks)
        {
            var serviceToRemove = services.FirstOrDefault(x => x.ServiceType == interfaceType);
            services.Remove(serviceToRemove!);
            services.AddSingleton(interfaceType, serviceMock);
        }
    }
}