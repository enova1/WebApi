using Hangfire;
using WebApi.Services.Hangfire;

namespace WebApi.Logic;

/// <inheritdoc />
public class JobScheduler : Recurring
{
    [AutomaticRetry(Attempts = 3)]
    public override void Job3(int clientId, string name, string email)
    {
        //Job Logic
        var estDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, EstTimeZone);
        NotificationService.NotifyProcessingCompletion($"Job3({email}) {clientId}:{name}: {estDateTime:MM/dd/yyyy hh:mm:ss tt}");
    }
}