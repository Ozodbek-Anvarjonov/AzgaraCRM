using AzgaraCRM.WebUI.Services.Interfaces;

namespace AzgaraCRM.WebUI.Services;

public class AssetService(IWebHostEnvironment environment) : IAssetService
{
    public async Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var folderName = "Uploads";
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var directoryPath = Path.Combine(environment.WebRootPath, folderName);
        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var fileStream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.OpenOrCreate);
        var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();
        await fileStream.WriteAsync(bytes, cancellationToken);

        return $"{folderName}/{fileName}";
    }
}