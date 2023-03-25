using Hangfire.Storage.Monitoring;
using Hangfire;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Logging;
using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.Background;

public class JobService : IJobService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IMediator _mediator;
    private readonly ILoggerManager<JobService> _loggerManager;

    public JobService(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IMediator mediator, ILoggerManager<JobService> loggerManager)
    {
        _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public void FireAndForgetJobMediator(ICommand<Unit> command)
    {
        _backgroundJobClient.Enqueue(() => _mediator.Send(command, default));
    }

    public void RecurringJobMediator(string name, ICommand<Unit> command, string cron)
    {
        _recurringJobManager.AddOrUpdate(name, () => _mediator.Send(command, default), cron);
    }

    public string ScheduleJobMediator(ICommand<Unit> command, TimeSpan delay)
    {
        return _backgroundJobClient.Schedule(() => _mediator.Send(command, default), delay);
    }

    /// <summary>
    /// Delete by name
    /// </summary> 
    /// <param name="name"></param> 
    /// <param name="successMessage"></param>
    public void DeleteBackgroundJobByUniqueJobName(string name, string successMessage)
    {
        var jobsProcessing = GetScheduledJob(name)?.SingleOrDefault();

        if (jobsProcessing == null)
        {
            throw new BadRequestException("Incorrect job name", "The job name is not unique or not found.");
        }

        BackgroundJob.Delete(jobsProcessing!.Value.Key);
        _loggerManager.LogInformation(successMessage);
    }


    /// <summary>
    /// Delete by argument
    /// </summary>
    /// <typeparam name="TJobRequestType"></typeparam>
    /// <param name="name"></param>
    /// <param name="funcComparer"></param>
    /// <param name="successMessage"></param>
    public void DeleteBackgroundJob<TJobRequestType>(string name, Func<TJobRequestType, bool> funcComparer,
        string successMessage)
    {
        var jobsProcessing = GetScheduledJob(name);

        if (jobsProcessing != null && jobsProcessing.Any())
        {
            foreach (var job in jobsProcessing)
            {
                var value = job.Value.Job.Args[0];
                if (value is TJobRequestType requestType)
                {
                    if (funcComparer.Invoke(requestType))
                    {
                        BackgroundJob.Delete(job.Key);
                        _loggerManager.LogInformation(successMessage);
                    }
                }
            }
        }
    }

    private List<KeyValuePair<string, ScheduledJobDto>>? GetScheduledJob(string methodName)
    {
        var monitor = JobStorage.Current.GetMonitoringApi();
        var jobsProcessing = monitor.ScheduledJobs(0, int.MaxValue)
            .Where(x => x.Value?.Job?.Method?.Name == methodName)?.ToList();

        return jobsProcessing;
    }
}