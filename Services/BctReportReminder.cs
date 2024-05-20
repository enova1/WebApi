using BCT_Scheduler;
using Hangfire;

namespace WebApi.Services;

public class BctReportReminder(ILogger<BctReportReminder> logger, string timeZone = "Eastern Standard Time")
{
    [AutomaticRetry(Attempts = 3)]
    public (bool, string) CreateReminder(int clientId, int templateId, string jobName, string cron, string queue = "email")
    {
        try
        {
            RecurringJob.AddOrUpdate<BctReport>($"{clientId}:{jobName}", x => x.SendReminderEmail(clientId, jobName, templateId), cron, queue: queue);
            logger.LogInformation($"{jobName} has been scheduled successfully: {TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone)):MM/dd/yyyy hh:mm:ss tt}");

            return (true, $"{jobName} has been scheduled successfully: {TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone)):MM/dd/yyyy hh:mm:ss tt}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);

            return (false, e.Message);
        }
    }
}