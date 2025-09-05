namespace FlixHub.Core.Services;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly string _asureBlobUri;

    public AzureBlobService(IAppSettingsKeyManagement appSettingsKeyManagement)
    {
        _asureBlobUri = appSettingsKeyManagement.AzurBlobServiceOptions.Uri;
        var azureBlobToken = appSettingsKeyManagement.AzurBlobServiceOptions.Token.Decrypt();

        _blobContainerClient = new BlobContainerClient(new Uri($"{_asureBlobUri}?{azureBlobToken}"), null);
    }

    public async Task<AzureBlobUploadResult> UploadAsync(AzureBlobUploadCommand command)
    {
        var blobId = Guid.NewGuid();
        var blobClient = _blobContainerClient.GetBlobClient(blobId.ToString());
        var binaryData = new BinaryData(command.FileContent);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders()
            {
                ContentType = command.MimeType,
            },
        };

        await blobClient.UploadAsync(binaryData, uploadOptions);

        return new AzureBlobUploadResult
        {
            Id = blobId,
            Uri = new($"{_asureBlobUri}/{blobClient.Name}"),
        };
    }

    public async Task<AzureBlobDeleteResult> DeleteAsync(AzureBlobDeleteCommand deleteRequest)
    {
        BlobClient blob = _blobContainerClient.GetBlobClient(deleteRequest.Id.ToString());
        bool exists = await blob.ExistsAsync();

        if (!exists)
        {
            return new AzureBlobDeleteResult
            {
                IsSuccess = false,
                Error = "Blob not found"
            };
        }

        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

        return new AzureBlobDeleteResult
        {
            IsSuccess = true,
        };
    }
}