namespace WebApi.Services;

/// <summary>
/// Notification Service Interface
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Notifies the completion of a Hangfire process
    /// </summary>
    /// <param name="message"></param>
    void NotifyProcessingCompletion(string message);
}