using MovieReviewApi.Application.Interfaces.Hangfire;

namespace MovieReviewApi.Infrastructure.Jobs
{
    public class DeleteLogsJob
    {
    private readonly ILogFileCleaner _logCleaner;
        public DeleteLogsJob(ILogFileCleaner logCleaner) { 
            _logCleaner = logCleaner;
        }

        public async Task Execute(CancellationToken cancellationToken) {
            await _logCleaner.DeleteOldLogsAsync(cancellationToken);
        }
    }
}
