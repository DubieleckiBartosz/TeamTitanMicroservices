using Moq;
using Shared.Implementations.RabbitMQ;
using System.Reflection;

namespace Calculator.IntegrationTests.Setup;

public class MockServices
{
    public Mock<IRabbitBase> RabbitBaseMock { get; }
    public Mock<IRabbitEventListener> RabbitEventListenerMock { get; }
    public MockServices()
    {
        RabbitBaseMock = new Mock<IRabbitBase>();
        RabbitEventListenerMock = new Mock<IRabbitEventListener>();
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