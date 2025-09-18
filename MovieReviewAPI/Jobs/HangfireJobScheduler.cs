using Hangfire;
using MovieReviewApi.Infrastructure.Jobs;

public static class HangfireJobScheduler
{
    public static void ScheduleJobs()
    {
        RecurringJob.AddOrUpdate<DeleteLogsJob>(
            recurringJobId: "delete-old-logs",                // unique ID
            methodCall: job => job.Execute(CancellationToken.None), // method to run
            cronExpression: "0 0 */7 * *"                   // every 7 days at midnight
        );
    }
}
