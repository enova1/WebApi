
namespace WebApi.Models;

public class BtcRptTemplateReminderRequest
{

    public required string JobName { get; set; }

    public required string Month { get; set; }

    public required int ReminderId { get; set; }

    public required CronExpressionModel CronExpressionModel { get; set; }

}