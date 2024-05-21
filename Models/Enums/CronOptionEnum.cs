using Hangfire;

namespace WebApi.Models.Enums;

public enum CronOptionEnum
{
    Minutely,
    Hourly,
    Daily,
    Weekly,
    Monthly,
    Yearly,
    Never
}

public static class CronOptionEnumExtensions
{
    /// <summary>
    /// Turns the CronOptionEnum into a Cron expression string
    /// </summary>
    /// <param name="cronOption"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToCronExpression(this CronOptionEnum cronOption)
    {
        return cronOption switch
        {
            CronOptionEnum.Minutely => Cron.Minutely(),
            CronOptionEnum.Hourly => Cron.Hourly(),
            CronOptionEnum.Daily => Cron.Daily(),
            CronOptionEnum.Weekly => Cron.Weekly(),
            CronOptionEnum.Monthly => Cron.Monthly(),
            CronOptionEnum.Yearly => Cron.Yearly(),
            CronOptionEnum.Never => Cron.Never(),
            _ => throw new ArgumentOutOfRangeException(nameof(cronOption))
        };
    }
}