using MediatR;
using Shared.Implementations.Abstractions;

namespace Shared.Implementations.Background;

public interface IJobService
{
    void FireAndForgetJobMediator(ICommand<Unit> command);
    void RecurringJobMediator(string name, ICommand<Unit> command, string cron);
    void DeleteBackgroundJobByUniqueJobName(string name, string successMessage);
    void DeleteBackgroundJob<TJobRequestType>(string name, Func<TJobRequestType, bool> funcComparer,
        string successMessage);
}