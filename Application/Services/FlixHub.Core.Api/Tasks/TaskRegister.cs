namespace FlixHub.Core.Api.Tasks;

internal static class TaskRegister
{
    public static WebApplication Register(this WebApplication app,
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

        return app;
    }
}
