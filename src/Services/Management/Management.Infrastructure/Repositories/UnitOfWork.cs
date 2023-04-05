using Management.Application.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Implementations.Dapper;
using Shared.Implementations.ProcessDispatcher;
using Shared.Implementations.Delegates;
using Shared.Domain.Base;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly string _connectionString;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ITransaction _transaction;
    private readonly IServiceProvider _serviceProvider;

    private ICompanyRepository? _companyRepository;
    private IDepartmentRepository? _departmentRepository;
    private IEmployeeRepository? _employeeRepository;
    private IDayOffRequestRepository? _dayOffRequestRepository;

    public UnitOfWork(string connectionString, IDomainEventDispatcher domainEventDispatcher, ITransaction transaction, IServiceProvider serviceProvider)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        this._domainEventDispatcher =
            domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));
        this._transaction = transaction;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public ICompanyRepository CompanyRepository
    {
        get
        {
            if (this._companyRepository == null)
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager<CompanyRepository>>();
                this._companyRepository = new CompanyRepository(this._connectionString, logger);
            }

            return this._companyRepository;
        }
    }

    public IDepartmentRepository DepartmentRepository
    {
        get
        {
            if (this._departmentRepository == null)
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager<DepartmentRepository>>();
                this._departmentRepository = new DepartmentRepository(this._connectionString, logger);
            }

            return this._departmentRepository;
        }
    }

    public IEmployeeRepository EmployeeRepository
    {
        get
        {
            if (this._employeeRepository == null)
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager<EmployeeRepository>>();
                this._employeeRepository = new EmployeeRepository(this._connectionString, logger);
            }

            return this._employeeRepository;
        }
    }
    
    public IDayOffRequestRepository DayOffRequestRepository
    {
        get
        {
            if (this._dayOffRequestRepository == null)
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager<DayOffRequestRepository>>();
                this._dayOffRequestRepository = new DayOffRequestRepository(this._connectionString, logger);
            }

            return this._dayOffRequestRepository;
        }
    }

    public async Task CompleteAsync<T>(T entity) where T : Entity
    {
        await this._domainEventDispatcher.DispatchEventsAsync(entity,
            new TransactionDelegates.CommitTransaction(this._transaction.Commit),
            new TransactionDelegates.RollbackTransaction(this._transaction.Rollback));
    }
}