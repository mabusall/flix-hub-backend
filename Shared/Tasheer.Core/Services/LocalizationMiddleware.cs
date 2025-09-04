namespace Tasheer.Core.Services;

public class LocalizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Get the language from Accept-Language header
        var acceptLanguage = context.Request.Headers.AcceptLanguage.ToString();

        // Split by commas to get individual language codes with optional quality values
        var language = acceptLanguage.Split(',')
            .Select(lang => lang.Split(';')[0])  // Take the part before the semicolon (if any)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(language))
            language = "ar"; // Fallback to default language if not provided

        // check what is the current language
        var currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();

        if (currentLanguage != language)
        {
            // Set the current culture and UI culture for the thread
            var cultureInfo = new CultureInfo(language);
            CultureInfo.CurrentCulture =
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        await next(context); // Call the next middleware in the pipeline
    }
}

public static class LocalizationInterpretor
{
    public static bool IsArabic()
        => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

    public static string CurrentLanguage()
        => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
}