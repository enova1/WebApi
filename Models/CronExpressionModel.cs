
namespace WebApi.Models;

/// <summary>
/// 
/// </summary>
/// <param name="minute"></param>
/// <param name="hour"></param>
/// <param name="dayOfMonth"></param>
/// <param name="month"></param>
/// <param name="dayOfWeek"></param>
/// <param name="year"></param>
public class CronExpressionModel()
{
    public string Minute { get; set; } = "*";
    public string Hour { get; set; } = "*";
    public string DayOfMonth { get; set; } = "*";
    public string Month { get; set; } = "*";
    public string DayOfWeek { get; set; } = "*";


    public override string ToString()
    {
        return $"{Minute} {Hour} {DayOfMonth} {Month} {DayOfWeek}";
    }
}


