namespace FlixHub.Notification.Views.Account;

public class VerifyOtpModel : PageModel
{
    [BindProperty]
    public string? OtpValue { get; set; }

    [BindProperty]
    public string? LanguageIsoCode { get; set; }

    [BindProperty]
    public string? SiteUrl { get; set; }
}
