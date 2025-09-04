namespace Mabusall.Notification.Consumer;

public class PushMobileNotificationConsumer(IMobileNotificationService mobileNotificationService)
    : IConsumer<MobileNotificationEvent>
{
    public async Task Consume(ConsumeContext<MobileNotificationEvent> context)
    {
        await mobileNotificationService.PushNotification(context.Message.Adapt<FirebaseNotificationMessage>());
    }
}