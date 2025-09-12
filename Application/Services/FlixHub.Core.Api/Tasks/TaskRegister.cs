namespace FlixHub.Core.Api.Tasks;

internal static class TaskRegister
{
    static void RegisterSyncMovieGenres(WebApplication app,
                                        IRecurringJobManager jobManager,
                                        HangfireOptions options)
    {
        var task = options!.Tasks["SyncMovieGenres"];
        var cron = task.Schedule.ToCronExpression();
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

    static void RegisterSyncContentLog(WebApplication app,
                                       IRecurringJobManager jobManager,
                                       HangfireOptions options)
    {
        var task = options!.Tasks["SyncContentLog"];
        var cron = task.Schedule.ToCronExpression();
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

    static void RegisterSyncContents(WebApplication app,
                                     IRecurringJobManager jobManager,
                                     HangfireOptions options)
    {
        var task = options!.Tasks["SyncContents"];
        var cron = task.Schedule.ToCronExpression();
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
        RegisterSyncMovieGenres(app, jobManager, options);
        RegisterSyncContentLog(app, jobManager, options);
        RegisterSyncContents(app, jobManager, options);

        return app;
    }
}
