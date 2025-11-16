namespace FlixHub.Core.Api.Tasks;

internal static class TaskRegister
{
    static void RegisterSyncMovieGenres(IRecurringJobManager jobManager,
                                        HangfireOptions options)
    {
        var task = options!.Tasks["SyncMovieGenres"];
        jobManager.RemoveIfExists(task.Id);
        if (task.IsEnabled)
        {
            TaskManager.RegisterHangfireJob<SyncMovieGenres>(jobManager,
                                                             task.Id,
                                                             task.Schedule.ToCronExpression(),
                                                             handler => handler.ExecuteAsync(),
                                                             task.AutoStart);
        }
    }

    static void RegisterSyncContentLog(IRecurringJobManager jobManager,
                                       HangfireOptions options)
    {
        var task = options!.Tasks["SyncContentLog"];
        jobManager.RemoveIfExists(task.Id);
        if (task.IsEnabled)
        {
            TaskManager.RegisterHangfireJob<SyncContentLog>(jobManager,
                                                            task.Id,
                                                            task.Schedule.ToCronExpression(),
                                                            handler => handler.ExecuteAsync(),
                                                            task.AutoStart);
        }
    }

    static void RegisterSyncContents(IRecurringJobManager jobManager,
                                     HangfireOptions options)
    {
        var task = options!.Tasks["SyncContents"];
        jobManager.RemoveIfExists(task.Id);
        if (task.IsEnabled)
        {
            TaskManager.RegisterHangfireJob<SyncContents>(jobManager,
                                                            task.Id,
                                                            task.Schedule.ToCronExpression(),
                                                            handler => handler.ExecuteAsync(),
                                                            task.AutoStart);
        }
    }

    public static WebApplication Register(this WebApplication app,
                                          IRecurringJobManager jobManager,
                                          HangfireOptions options)
    {
        RegisterSyncMovieGenres(jobManager, options);
        RegisterSyncContentLog(jobManager, options);
        RegisterSyncContents(jobManager, options);

        return app;
    }
}
