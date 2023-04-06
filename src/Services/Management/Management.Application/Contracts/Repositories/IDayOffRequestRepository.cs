using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IDayOffRequestRepository
{
    Task<DayOffRequest?> GetDayOffRequestByIdAsync(int dayOffRequestId);
    Task CancelDayOffRequestAsync(DayOffRequest dayOffRequest);
    Task ConsiderDayOffRequestAsync(DayOffRequest dayOffRequest);
}