using Hangfire;
using Identity.Application.Constants;
using Identity.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Services;

public class BackgroundService : IBackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRecurringJobManager _recurringJobManager;

    public BackgroundService(IServiceScopeFactory scopeFactory, IRecurringJobManager recurringJobManager)
    {
        _scopeFactory = scopeFactory;
        _recurringJobManager = recurringJobManager; 
    } 

    public void StartJobs()
    {
        ClearTokensRecurringJob();
        ClearTempUsersRecurringJob();
    }

    private void ClearTokensRecurringJob()
    {
        using var scope = _scopeFactory.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        _recurringJobManager.AddOrUpdate(ConstantKeys.ClearTokensRecurringJob,
            () => userRepository.ClearTokens(), "0 */12 * * *");
    }

    private void ClearTempUsersRecurringJob()
    {
        using var scope = _scopeFactory.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        _recurringJobManager.AddOrUpdate(ConstantKeys.ClearTempUsersRecurringJob,
            () => userRepository.ClearTempUsers(DateTime.UtcNow.AddMonths(-1)), "0 */12 * * *");
    }
}