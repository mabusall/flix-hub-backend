using System.Diagnostics;
using System.Globalization;

namespace FlixHub.Resources;

[DebuggerNonUserCode()]
[System.Runtime.CompilerServices.CompilerGenerated()]
public class ErrorMessageResources
{
    private static System.Resources.ResourceManager? resourceMan;
    private static CultureInfo? resourceCulture;

    internal ErrorMessageResources()
    {

    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Resources.ResourceManager ResourceManager
    {
        get
        {
            if (ReferenceEquals(resourceMan, null))
            {
                System.Resources.ResourceManager temp = new("FlixHub.Resources.ErrorMessageResources", typeof(ErrorMessageResources).Assembly);
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

    public static string NotFound
        => ResourceManager.GetString("NotFound", resourceCulture)!;
    public static string NotNull
        => ResourceManager.GetString("NotNull", resourceCulture)!;
    public static string NotEmpty
        => ResourceManager.GetString("NotEmpty", resourceCulture)!;
    public static string ValueMustBeGreaterThanZero
        => ResourceManager.GetString("ValueMustBeGreaterThanZero", resourceCulture)!;
    public static string EnumIsNotDefined
        => ResourceManager.GetString("EnumIsNotDefined", resourceCulture)!;

    public static string UserManagement_EmailNotFound
        => ResourceManager.GetString("UserManagement_EmailNotFound", resourceCulture)!;
    public static string UserManagement_WeakPassword
        => ResourceManager.GetString("UserManagement_WeakPassword", resourceCulture)!;
    public static string UserManagement_InvalidEmailAddress
        => ResourceManager.GetString("UserManagement_InvalidEmailAddress", resourceCulture)!;
    public static string UserManagement_EmailAlreadyExists
        => ResourceManager.GetString("UserManagement_EmailAlreadyExists", resourceCulture)!;
    public static string UserManagement_EmailNotRegistered
        => ResourceManager.GetString("UserManagement_EmailNotRegistered", resourceCulture)!;
    public static string UserManagement_InvalidVerificationCode
        => ResourceManager.GetString("UserManagement_InvalidVerificationCode", resourceCulture)!;
    public static string UserManagement_UsernameAlreadyExists
        => ResourceManager.GetString("UserManagement_UsernameAlreadyExists", resourceCulture)!;
    public static string UserManagement_InvalidEmailOrPassword
        => ResourceManager.GetString("UserManagement_InvalidEmailOrPassword", resourceCulture)!;
}
