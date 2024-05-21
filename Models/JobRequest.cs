
using WebApi.Models.Enums;

namespace WebApi.Models;

public class JobRequest
{
    public required int ClientId { get; set; }

    public required string Name { get; set; }

    public required int TemplateId { get; set; }

    /// <summary>
    /// CronOptionEnum (hangfire.Cron) to set the Cron by option
    /// </summary>
    public CronOptionEnum? CronOption { get; set; }

    /// <summary>
    /// Pass in the CronExpression to set the Cron by expression
    /// </summary>
    public CronExpressionModel? CronExpressionModel { get; set; }

}