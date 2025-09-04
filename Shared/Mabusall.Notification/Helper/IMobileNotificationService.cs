namespace Mabusall.Notification.Helper;
public interface IMobileNotificationService
{
    Task<bool> PushNotification(FirebaseNotificationMessage request);
}