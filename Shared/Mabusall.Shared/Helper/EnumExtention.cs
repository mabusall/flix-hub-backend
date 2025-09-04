namespace Mabusall.Shared.Helper;

public static class EnumExtention
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length != 0)
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }

    public static string GetDisplay(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        if (fi is null) return value.ToString();

        if (fi.GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] attributes && attributes.Length != 0)
        {
            return attributes.First().Name;
        }

        return value.ToString();
    }

    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase))
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name.ToLower() == description.ToLower())
                    return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
    }
}