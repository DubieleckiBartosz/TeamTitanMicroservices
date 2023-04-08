using AutoFixture;
using Calculator.Application.Contracts.Repositories;
using Moq;
using Moq.AutoMock;
using Shared.Domain.Base;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Background;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;

namespace Calculator.UnitTests.Application.HandlerTests;

public class CommandHandlerBaseTests<THandler, TRequest, TResponse, TAggregateType> 
    where TRequest : ICommand<TResponse>
    where THandler : class, ICommandHandler<TRequest, TResponse>
    where TAggregateType: Aggregate
{
    protected Mock<ICurrentUser> CurrentUserMock;
    protected Mock<IAccountRepository> AccountRepositoryMock; 
    protected Mock<IProductRepository> ProductRepositoryMock; 
    protected Mock<IJobService> JobServiceMock; 
    protected Mock<IRepository<TAggregateType>> AggregateRepositoryMock; 
    protected readonly Fixture Fixture;
    protected readonly AutoMocker Mocker; 
    protected readonly THandler Handler;

    public CommandHandlerBaseTests()
    {
        this.Mocker = new AutoMocker();
        this.Fixture = new Fixture();
        this.CurrentUserMock = this.Mocker.GetMock<ICurrentUser>();
        this.AccountRepositoryMock = this.Mocker.GetMock<IAccountRepository>();
        this.ProductRepositoryMock = this.Mocker.GetMock<IProductRepository>();
        this.JobServiceMock = this.Mocker.GetMock<IJobService>();
        this.AggregateRepositoryMock = this.Mocker.GetMock<IRepository<TAggregateType>>();
        this.Handler = this.Mocker.CreateInstance<THandler>();
    }
    protected int GetRandomInt(int a = 1, int b = 10) => new Random().Next(a, b);

    protected string AggregateNullMessageError(string name) => $"{name ?? "Aggregate"} cannot be null";
}