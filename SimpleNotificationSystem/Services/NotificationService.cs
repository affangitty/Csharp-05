using SimpleNotificationSystem.Interfaces;
using SimpleNotificationSystem.Models;

namespace SimpleNotificationSystem.Services;

public class NotificationService
{
    private readonly INotificationSender _emailSender;
    private readonly INotificationSender _smsSender;
    private readonly INotificationSender? _whatsAppSender;

    public NotificationService(
        INotificationSender emailSender,
        INotificationSender smsSender,
        INotificationSender? whatsAppSender = null)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _whatsAppSender = whatsAppSender;
    }

    public void SendEmail(User user, Notification notification)
    {
        StampSentDate(notification);
        _emailSender.Send(user, notification);
    }

    public void SendSms(User user, Notification notification)
    {
        StampSentDate(notification);
        _smsSender.Send(user, notification);
    }

    public void SendWhatsApp(User user, Notification notification)
    {
        if (_whatsAppSender is null)
            throw new InvalidOperationException(
                "WhatsApp is not registered. Pass a WhatsAppNotification (or any INotificationSender) as the third constructor argument.");

        StampSentDate(notification);
        _whatsAppSender.Send(user, notification);
    }

    public void Send(User user, Notification notification, INotificationSender channel)
    {
        StampSentDate(notification);
        channel.Send(user, notification);
    }

    private static void StampSentDate(Notification notification) =>
        notification.SentDate = DateTime.Now;
}
