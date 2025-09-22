namespace MovieReviewApi.Application.Interfaces.Hangfire
{
    public interface ILogFileCleaner
    {
        Task DeleteOldLogsAsync(CancellationToken cancellationToken);
    }
}
