using BCT_Scheduler;
using Hangfire;
using Models.Hangfire;
using Models.Hangfire.Enums;
using WebApi.Services.Helpers;

namespace WebApi.Services;

public class BctReportReminder(ILogger<BctReportReminder> logger, string timeZone = "Eastern Standard Time")
{
    public (bool, string) CreateReminder(int clientId, int templateId, string jobName, CronOptionEnum cron, string queue = "email")
    {
        return CreateReminder(clientId, templateId, jobName, CronExpressionBuilder.Build(cron), queue);
    }
    public (bool, string) CreateReminder(int clientId, int templateId, string jobName, CronExpressionModel cron, string queue = "email")
    {
        return CreateReminder(clientId, templateId, jobName, CronExpressionBuilder.Build(cron), queue);
    }
    [AutomaticRetry(Attempts = 3)]
    private (bool, string) CreateReminder(int clientId, int templateId, string jobName, string cron, string queue)
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