using Hangfire;

namespace WebApi.Services.Hangfire;

public class Delayed
{
    [AutomaticRetry(Attempts = 3)]
    public void SendReminder(string recipient, string body)
    {
        //Send Logic
        Console.WriteLine("DONE");
    }
}