using Hangfire;
using Models.Hangfire;
using Models.Hangfire.Enums;

namespace WebApi.Services.Hangfire.Helpers;

/// <summary>
/// Builds a CRON expression based on the provided parameters.
/// Usage: string cronExpression = CronExpressionBuilder.Build(0, 12, "*", "*", "MON-FRI");
///         The CRON expression 0 12 * * 1-5 represents a schedule that runs a task at 12:00 PM (noon) every day from Monday to Friday.
/// </summary>
public class CronExpressionBuilder
{
    /// <summary>
    /// Builds a CRON expression based on the provided parameters.
    /// </summary>
    /// <param name="cronExpression"></param>
    /// <returns></returns>
    public static string Build(CronExpressionModel cronExpression)
    {
        return ValidateAndFormat(cronExpression.ToString());
    }
    /// <summary>
    /// Builds a CRON expression based on Hangfire's Cron object...
    /// </summary>
    /// <param name="cronOption"></param>
    /// <returns></returns>
    public static string Build(CronOptionEnum cronOption)
    {
        string cronString = cronOption switch
        {
            CronOptionEnum.Minutely => "* * * * *",
            CronOptionEnum.Hourly => "0 * * * *",
            CronOptionEnum.Monthly => "0 0 1 * *",
            CronOptionEnum.Daily => "0 0 * * *",
            CronOptionEnum.Weekly => "0 0 * * 0",
            CronOptionEnum.Yearly => "0 0 1 1 *",
            _ => throw new ArgumentOutOfRangeException(nameof(cronOption), cronOption, null)
        };

        string cronExpression = $"{cronString}";
        return ValidateAndFormat(cronExpression);
    }

    private static string ValidateAndFormat(string cronExpression)
    {
        string[] parts = cronExpression.Split(' ');
        if (parts.Length != 5)
        {
            throw new ArgumentException("Invalid number of CRON expression parts.");
        }

        // Validate each part according to CRON rules
        ValidateCronPart(parts[0], 0, 59, "minute");
        ValidateCronPart(parts[1], 0, 23, "hour");
        ValidateCronPart(parts[2], 1, 31, "day of month");
        ValidateCronPart(parts[3], 1, 12, "month");
        ValidateCronPart(parts[4], 0, 7, "day of week");

        return cronExpression;
    }

    private static void ValidateCronPart(string part, int minValue, int maxValue, string partName)
    {
        if (part == "*")
        {
            return; // wildcard (*) is always valid
        }

        if (!int.TryParse(part, out int value) || value < minValue || value > maxValue)
        {
            throw new ArgumentException($"Invalid value for {partName}: {part}");
        }
    }
}