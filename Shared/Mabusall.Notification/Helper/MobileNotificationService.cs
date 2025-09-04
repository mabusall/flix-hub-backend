namespace Mabusall.Notification.Helper;

public class MobileNotificationService(IAppSettingsKeyManagement appSettingsKeyManagement) : IMobileNotificationService
{
    public async Task<bool> PushNotification(FirebaseNotificationMessage request)
    {
        var androidOptions = appSettingsKeyManagement.FirebaseOptions.AndroidConfiguration;

        dynamic response;
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;

        var data = new Dictionary<string, string>
        {
            { "MessageTitleAr", request.MessageTitleAr! },
            { "MessageBodyAr", request.MessageBodyAr! },
            { "MessageTitleEn", request.MessageTitleEn! },
            { "MessageBodyEn", request.MessageBodyEn! },
            { "ImageUrl", request.ImageUrl! }
        };

        if (request.DeviceTokens is null || request.DeviceTokens.Count == 0)
        {
            var firebaseMessage = new Message()
            {
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    //Notification = new AndroidNotification()
                    //{
                    //    ChannelId = androidOptions.ChannelId,
                    //    ClickAction = androidOptions.ClickAction,
                    //},
                },
                Data = data,
                Topic = "allUsers"
            };

            response = await firebaseMessaging.SendAsync(firebaseMessage);

            return !string.IsNullOrWhiteSpace(response);
        }

        var firebaseMulticastMessage = new MulticastMessage()
        {
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                //Notification = new AndroidNotification()
                //{
                //    ChannelId = androidOptions.ChannelId,
                //    ClickAction = androidOptions.ClickAction,
                //},
            },
            Data = data,
            Tokens = request.DeviceTokens
        };

        response = await firebaseMessaging.SendEachForMulticastAsync(firebaseMulticastMessage);

        return response.SuccessCount > 0;
    }
}