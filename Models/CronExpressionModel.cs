
namespace WebApi.Models;


public abstract class CronExpressionModel()
{
    private string Minute { get; set; } = "*";
    private string Hour { get; set; } = "*";
    private string DayOfMonth { get; set; } = "*";
    private string Month { get; set; } = "*";
    private string DayOfWeek { get; set; } = "*";


    public override string ToString()
    {
        return $"{Minute} {Hour} {DayOfMonth} {Month} {DayOfWeek}";
    }
}