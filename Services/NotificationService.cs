namespace WebApi.Services;

public abstract class NotificationService
{
    public static void NotifyProcessingCompletion()
    {
        Console.WriteLine("DONE");
    }
}