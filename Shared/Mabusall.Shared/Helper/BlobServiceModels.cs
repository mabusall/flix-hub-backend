namespace Tasheer.Shared.Helper;

public class AzureBlobUploadCommand
{
    required public string MimeType { get; set; }
    required public byte[] FileContent { get; set; }
}

public class AzureBlobUploadResult
{
    public Guid Id { get; set; }
    public Uri Uri { get; set; }
}

public class AzureBlobDeleteCommand
{
    required public Guid Id { get; set; }
}

public class AzureBlobDeleteResult
{
    public bool IsSuccess { get; set; }

    public string Error { get; set; }
}