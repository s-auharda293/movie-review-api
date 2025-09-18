using MovieReviewApi.Application.Interfaces.Hangfire;

namespace MovieReviewApi.Infrastructure.Services
{
    public class LogFileCleaner : ILogFileCleaner
    {
        private readonly string _logDirectory;

        public LogFileCleaner()
        {
            _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        }
        public Task DeleteOldLogsAsync(CancellationToken cancellationToken)
        {

            if (!Directory.Exists(_logDirectory)) {
                return Task.CompletedTask;
            }
            var files = Directory.GetFiles(_logDirectory);
            foreach (var file in files) {
                cancellationToken.ThrowIfCancellationRequested();

                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTime < DateTime.Now.AddDays(-7)) { 
                    fileInfo.Delete();
                }
            }

            return Task.CompletedTask;

        }
    }
}
