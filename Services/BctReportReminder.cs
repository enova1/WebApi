using BCT_Scheduler;
using Hangfire;
using System.Text.RegularExpressions;
using WebApi.Models;
using WebApi.Services.Helpers;


namespace WebApi.Services;

public class BctReportReminder(ILogger<BctReportReminder> logger, string timeZone = "Eastern Standard Time")
{
    private readonly CronValidator _cronValidator = new ();
    [AutomaticRetry(Attempts = 3)]
    public (bool, string) CreateReminder(string month, int reminderId, string jobName, CronExpressionModel cronExpression, string queue = "email")
    {
        try
        {
            var cron = _cronValidator.IsValidCronExpression(cronExpression.ToString());

            RecurringJob.AddOrUpdate<BctReport>($"{jobName}", x => x.SendReminderEmail(reminderId, month), cron, queue: queue);

            var str = $"{jobName} has been scheduled successfully: {TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone)):MM/dd/yyyy hh:mm:ss tt}";
            logger.LogInformation(str);
            return (true, str);
        }
        catch (Exception e)
        {
            var str = $"Job was not scheduled for report reminder {reminderId}: {e.Message}";
            logger.LogError(e, str);
            return (false, str);
        }
    }

    public (bool, string) RemoveReminder(string jobName)
    {
        try
        {
            RecurringJob.RemoveIfExists($"{jobName}");
            logger.LogInformation($"{jobName} has been successfully Removed: {TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone)):MM/dd/yyyy hh:mm:ss tt}");

            return (true, "Success");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);

            return (false, e.Message);
        }
    }

}