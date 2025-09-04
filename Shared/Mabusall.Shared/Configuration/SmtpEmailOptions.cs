namespace Mabusall.Shared.Configuration;

public class SmtpEmailOptions
{
    public static string ConfigurationKey => "SmtpEmail";

    public string Server { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public int Timeout { get; set; }
    public bool EnableSsl { get; set; }
    public string SenderEmail { get; set; }
}
