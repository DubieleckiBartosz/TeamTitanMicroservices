using Dapper;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;

namespace Management.Infrastructure.Repositories;

public class DayOffRequestRepository : BaseRepository<DayOffRequestRepository>, IDayOffRequestRepository
{
    public DayOffRequestRepository(string dbConnection, ILoggerManager<DayOffRequestRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<DayOffRequestDao?> GetDayOffRequestByIdAsync(int dayOffRequestId)
    {
        var param = new DynamicParameters();

        param.Add("@dayOffRequestId", dayOffRequestId);

        var result = (await QueryAsync<DayOffRequestDao>("dayOff_getById_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result;
    }

    public async Task CancelDayOffRequestAsync(DayOffRequest dayOffRequest)
    {
        var param = new DynamicParameters();

        param.Add("@dayOffRequestId", dayOffRequest.Id);
        param.Add("@isCanceled", dayOffRequest.Canceled);
        param.Add("@version", dayOffRequest.Version);

        var result = await ExecuteAsync("dayOff_cancel_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'dayOff_cancel_U' failed", "Database Error");
        }
    }

    public async Task ConsiderDayOffRequestAsync(DayOffRequest dayOffRequest)
    {
        var param = new DynamicParameters();

        param.Add("@dayOffRequestId", dayOffRequest.Id);
        param.Add("@status", dayOffRequest.CurrentStatus.Id);
        param.Add("@consideredBy", dayOffRequest.ConsideredBy);
        param.Add("@version", dayOffRequest.Version);

        var result = await ExecuteAsync("dayOff_considerRequest_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'dayOff_considerRequest_U' failed", "Database Error");
        }
    }
}