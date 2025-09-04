using System.Diagnostics;
using System.Globalization;

namespace Tasheer.Resources;

[DebuggerNonUserCode()]
[System.Runtime.CompilerServices.CompilerGenerated()]
public class EmailNotificationResources
{
    private static System.Resources.ResourceManager? resourceMan;
    private static CultureInfo? resourceCulture;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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
                System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Tasheer.Resources.EmailNotificationResources", typeof(EmailNotificationResources).Assembly);
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

    public static string VerifyEmailTemplate_VerifyEmailTitle
        => ResourceManager.GetString("VerifyEmailTemplate_VerifyEmailTitle", resourceCulture)!;
    public static string VerifyEmailTemplate_YourOTPLabel
        => ResourceManager.GetString("VerifyEmailTemplate_YourOTPLabel", resourceCulture)!;
    public static string VerifyEmailTemplate_PleaseDoNotShareOTP
        => ResourceManager.GetString("VerifyEmailTemplate_PleaseDoNotShareOTP", resourceCulture)!;
    public static string VerifyEmailTemplate_OTPValidFor5min
        => ResourceManager.GetString("VerifyEmailTemplate_OTPValidFor5min", resourceCulture)!;
}
