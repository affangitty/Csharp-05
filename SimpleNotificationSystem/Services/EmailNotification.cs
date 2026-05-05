using SimpleNotificationSystem.Interfaces;
using SimpleNotificationSystem.Models;

namespace SimpleNotificationSystem.Services;

public class EmailNotification : INotificationSender
{
    public void Send(User user, Notification notification)
    {
        Console.WriteLine(
            $"[EMAIL] To: {user.Email} | Message: {notification.Message} | Sent: {notification.SentDate:yyyy-MM-dd HH:mm:ss}");
    }
}
