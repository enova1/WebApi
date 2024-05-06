namespace WebApi.Services;

public class NotificationService
{
    public static object NotifyProcessingCompletion()
    {
        Console.WriteLine("DONE");
        return null!;
    }
}