using System.Diagnostics;
using System.Globalization;

namespace Mabusall.Resources;

[DebuggerNonUserCode()]
[System.Runtime.CompilerServices.CompilerGenerated()]
public class EmailNotificationResources
{
    private static System.Resources.ResourceManager? resourceMan;
    private static CultureInfo? resourceCulture;

    internal EmailNotificationResources()
    {

    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Resources.ResourceManager ResourceManager
    {
        get
        {
            if (ReferenceEquals(resourceMan, null))
            {
                System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Mabusall.Resources.EmailNotificationResources", typeof(EmailNotificationResources).Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
        get
        {
            return resourceCulture!;
        }
        set
        {
            resourceCulture = value;
        }
    }

    public static string Team
        => ResourceManager.GetString("Team", resourceCulture)!;
    public static string DoNotReplyToThisMail
        => ResourceManager.GetString("DoNotReplyToThisMail", resourceCulture)!;
    public static string Copyright
        => ResourceManager.GetString("Copyright", resourceCulture)!;
    public static string WelcomeMessage
        => ResourceManager.GetString("WelcomeMessage", resourceCulture)!;

    public static string VerifyOtpTemplate_Title
        => ResourceManager.GetString("VerifyOtpTemplate_Title", resourceCulture)!;
    public static string VerifyOtpTemplate_YourOtpLabel
        => ResourceManager.GetString("VerifyOtpTemplate_YourOtpLabel", resourceCulture)!;
    public static string VerifyOtpTemplate_PleaseDoNotShareOTP
        => ResourceManager.GetString("VerifyOtpTemplate_PleaseDoNotShareOTP", resourceCulture)!;
    public static string VerifyOtpTemplate_OtpValidFor5min
        => ResourceManager.GetString("VerifyOtpTemplate_OtpValidFor5min", resourceCulture)!;

    public static string VerifyEmailTemplate_Title
        => ResourceManager.GetString("VerifyEmailTemplate_Title", resourceCulture)!;
    public static string VerifyEmailTemplate_SpAdmin_Body
        => ResourceManager.GetString("VerifyEmailTemplate_SpAdmin_Body", resourceCulture)!;
    public static string VerifyEmailTemplate_SpUser_Body
        => ResourceManager.GetString("VerifyEmailTemplate_SpUser_Body", resourceCulture)!;

    public static string ForgetPasswordTemplate_Title
        => ResourceManager.GetString("ForgetPasswordTemplate_Title", resourceCulture)!;
    public static string ForgetPasswordTemplate_Body
        => ResourceManager.GetString("ForgetPasswordTemplate_Body", resourceCulture)!;
    public static string NeedHelp
        => ResourceManager.GetString("NeedHelp", resourceCulture)!;
    public static string NeedForHelpMessage
        => ResourceManager.GetString("NeedForHelpMessage", resourceCulture)!;

    public static string ActivateSubscriptionTemplate_Title
        => ResourceManager.GetString("ActivateSubscriptionTemplate_Title", resourceCulture)!;
    public static string ActivateSubscriptionTemplate_Body
        => ResourceManager.GetString("ActivateSubscriptionTemplate_Body", resourceCulture)!;
    public static string RoomDistribution_MessageTitle
        => ResourceManager.GetString("RoomDistribution_MessageTitle", resourceCulture)!;
    public static string RoomDistribution_MessageBody
        => ResourceManager.GetString("RoomDistribution_MessageBody", resourceCulture)!;

    public static string ActivityAdded_MessageTitle
        => ResourceManager.GetString("ActivityAdded_MessageTitle", resourceCulture)!;
    public static string ActivityAdded_Body
        => ResourceManager.GetString("ActivityAdded_MessageBody", resourceCulture)!;
    public static string ActivityUpdated_MessageTitle
        => ResourceManager.GetString("ActivityUpdated_MessageTitle", resourceCulture)!;
    public static string ActivityUpdated_MessageBody
        => ResourceManager.GetString("ActivityUpdated_MessageBody", resourceCulture)!;
    public static string ActivityRemoved_MessageTitle
        => ResourceManager.GetString("ActivityRemoved_MessageTitle", resourceCulture)!;
    public static string ActivityRemoved_MessageBody
        => ResourceManager.GetString("ActivityRemoved_MessageBody", resourceCulture)!;
}
