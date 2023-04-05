using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class DayOffRequestRepository : BaseRepository<DayOffRequestRepository>, IDayOffRequestRepository
{
    public DayOffRequestRepository(string dbConnection, ILoggerManager<DayOffRequestRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<DayOffRequest> GetDayOffRequestByIdAsync(int dayOffRequestId)
    {
        throw new NotImplementedException();
    }

    public Task CancelDayOffRequestAsync(DayOffRequest dayOffRequest)
    {
        throw new NotImplementedException();
    }

    public Task ConsiderDayOffRequestAsync(DayOffRequest dayOffRequest)
    {
        throw new NotImplementedException();
    }
}