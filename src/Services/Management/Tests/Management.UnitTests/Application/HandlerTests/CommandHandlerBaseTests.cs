using AutoFixture;
using Management.Application.Contracts.Repositories;
using Moq;
using Moq.AutoMock;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Services;

namespace Management.UnitTests.Application.HandlerTests;

public class CommandHandlerBaseTests<THandler, TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where THandler : class, ICommandHandler<TRequest, TResponse>
{
    protected readonly Fixture Fixture;
    protected readonly AutoMocker Mocker;
    protected readonly THandler Handler;

    protected Mock<ICurrentUser> CurrentUserMock;
    protected Mock<ITransaction> TransactionMock;
    protected Mock<IUnitOfWork> UnitOfWork;
    protected Mock<ICompanyRepository> CompanyRepositoryMock;
    protected Mock<IEmployeeRepository> EmployeeRepositoryMock;
    protected Mock<IDepartmentRepository> DepartmentRepositoryMock;
    protected Mock<IDayOffRequestRepository> DayOffRequestRepositoryMock;
    protected Mock<IContractRepository> ContractRepositoryMock;

    public CommandHandlerBaseTests()
    {
        this.Mocker = new AutoMocker();
        this.Fixture = new Fixture();
        this.CurrentUserMock = this.Mocker.GetMock<ICurrentUser>();
        this.UnitOfWork = this.Mocker.GetMock<IUnitOfWork>();
        this.TransactionMock = this.Mocker.GetMock<ITransaction>();
        this.CompanyRepositoryMock = this.Mocker.GetMock<ICompanyRepository>();
        this.EmployeeRepositoryMock = this.Mocker.GetMock<IEmployeeRepository>();
        this.DepartmentRepositoryMock = Mocker.GetMock<IDepartmentRepository>();
        this.DayOffRequestRepositoryMock = Mocker.GetMock<IDayOffRequestRepository>();
        this.ContractRepositoryMock = Mocker.GetMock<IContractRepository>();

        this.UnitOfWork.SetupGet(_ => _.CompanyRepository).Returns(CompanyRepositoryMock.Object);
        this.UnitOfWork.SetupGet(_ => _.EmployeeRepository).Returns(EmployeeRepositoryMock.Object);
        this.UnitOfWork.SetupGet(_ => _.DepartmentRepository).Returns(DepartmentRepositoryMock.Object);
        this.UnitOfWork.SetupGet(_ => _.DayOffRequestRepository).Returns(DayOffRequestRepositoryMock.Object);
        this.UnitOfWork.SetupGet(_ => _.ContractRepository).Returns(ContractRepositoryMock.Object);
        this.TransactionMock.Setup(_ => _.GetTransactionWhenExist()).Returns(() => null);
        this.CurrentUserMock.SetupGet(_ => _.VerificationCode).Returns(() => Fixture.Create<string>());
        this.CurrentUserMock.SetupGet(_ => _.OrganizationCode).Returns(() => Fixture.Create<string>());

        this.Handler = Mocker.CreateInstance<THandler>();
    }
}