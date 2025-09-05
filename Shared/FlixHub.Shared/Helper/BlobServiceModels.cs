namespace FlixHub.Shared.Helper;

public class AzureBlobUploadCommand
{
    public required string MimeType { get; set; }
    public required byte[] FileContent { get; set; }
}

public class AzureBlobUploadResult
{
    public Guid Id { get; set; }
    public Uri Uri { get; set; }
}

public class AzureBlobDeleteCommand
{
    public required Guid Id { get; set; }
}

public class AzureBlobDeleteResult
{
    public bool IsSuccess { get; set; }

    public string Error { get; set; }
}