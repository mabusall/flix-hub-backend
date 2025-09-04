namespace Mabusall.Core.Tasks;

public static class TaskManager
{
    public static void AddRecurringTask(IHangfireTask task)
    {
        RecurringJob.RemoveIfExists(task.JobId);
        RecurringJob.AddOrUpdate(task.JobId, () => task.ExecuteAsync(), task.Cron);
        RecurringJob.TriggerJob(task.JobId);
    }

    public static void AddExecuteOnceTask(IHangfireTask handler)
    {
        BackgroundJob.Enqueue(() => handler.ExecuteAsync());
    }

    public static void RegisterHangfireJob<THandler>(IRecurringJobManager recurringJobManager,
                                          string jobKey,
                                          string cron,
                                          Expression<Func<THandler, Task>> expression,
                                          bool immediateRun = false)
        where THandler : class
    {
        recurringJobManager.RemoveIfExists(jobKey);
        recurringJobManager.AddOrUpdate(jobKey, expression, cron);

        if (immediateRun) RecurringJob.TriggerJob(jobKey);
    }
}