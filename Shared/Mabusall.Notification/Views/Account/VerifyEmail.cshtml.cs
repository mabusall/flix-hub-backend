namespace Mabusall.Notification.Views.Account;

public class VerifyEmailModel : PageModel
{
    [BindProperty]
    public string? RedirectUrl { get; set; }

    [BindProperty]
    public string? ServiceProviderName { get; set; }

    [BindProperty]
    public int UserTypeId { get; set; }

    [BindProperty]
    public string? LanguageIsoCode { get; set; }

    [BindProperty]
    public string? SiteUrl { get; set; }
}
