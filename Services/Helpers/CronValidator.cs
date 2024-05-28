using NCrontab;

namespace WebApi.Services.Helpers;

public class CronValidator
{
    public string IsValidCronExpression(string cronExpression)
    {
        try
        {
            CrontabSchedule.Parse(cronExpression);
            return cronExpression;
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Invalid CRON expression: {ex.Message}");
        }
    }
}