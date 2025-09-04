using System.Diagnostics;
using System.Globalization;

namespace Mabusall.Resources;

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
                System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Tasheer.Resources.ErrorMessageResources", typeof(ErrorMessageResources).Assembly);
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
}
