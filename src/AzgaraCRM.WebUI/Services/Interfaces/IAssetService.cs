namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IAssetService
{
    Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
}