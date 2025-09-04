namespace Tasheer.Shared.Interfaces;

public interface IAzureBlobService
{
    public Task<AzureBlobUploadResult> UploadAsync(AzureBlobUploadCommand command);
    public Task<AzureBlobDeleteResult> DeleteAsync(AzureBlobDeleteCommand deleteRequest);
}