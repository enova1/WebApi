using Hangfire;

namespace WebApi.Services.Hangfire;

public class Recurring
{
    protected readonly NotificationService NotificationService = new ();
    protected readonly TimeZoneInfo EstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    [AutomaticRetry(Attempts = 3)]
    public void Job1(int clientId, string name)
    {
        //TODO: Make Virtual and Move to derived class
        var estDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, EstTimeZone);
        NotificationService.NotifyProcessingCompletion($"Job1 {clientId}:{name}: {estDateTime:MM/dd/yyyy hh:mm:ss tt}");
    }

    [AutomaticRetry(Attempts = 3)]
    public void Job2(int clientId, string name)
    {
        //TODO: Make Virtual and Move to derived class
        var estDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, EstTimeZone);
        NotificationService.NotifyProcessingCompletion($"Job2 {clientId}:{name}: {estDateTime:MM/dd/yyyy hh:mm:ss tt}");
    }

    [AutomaticRetry(Attempts = 3)]
    public virtual void Job3(int clientId, string name, string email) { }
}