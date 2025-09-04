namespace Mabusall.Shared.Interfaces;

public interface IHangfireTask
{
    string JobId { get; }
    string Cron { get; }
    Task ExecuteAsync();
}

public interface IHangfireJob
{
    Task ExecuteAsync();
}