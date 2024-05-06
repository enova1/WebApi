using Hangfire;

namespace WebApi.Services.Hangfire;

public class Recurring
{
    [AutomaticRetry(Attempts = 3)]
    public void SendDailyEmail()
    {
        //Send Logic
        Console.WriteLine("DONE");
    }
}