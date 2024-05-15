namespace WebApi.Services;

/// <summary>
/// Notification Service
/// </summary>
public class NotificationService : INotificationService
{
    /// <summary>
    /// Notifies the completion of a Hangfire process
    /// </summary>
    /// <param name="message"></param>
    public void NotifyProcessingCompletion(string message)
    {
        Console.WriteLine($"Completed: {message}");
    }
}