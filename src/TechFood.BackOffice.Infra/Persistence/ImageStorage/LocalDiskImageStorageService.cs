using System.IO;
using System.Threading.Tasks;
using TechFood.BackOffice.Application.Common.Services.Interfaces;

namespace TechFood.Infra.Persistence.ImageStorage;

internal class LocalDiskImageStorageService : IImageStorageService
{
    private const string ImageFolderName = "images";

    public Task DeleteAsync(string fileName, string folder)
    {
        if (!string.IsNullOrWhiteSpace(folder))
        {
            folder = folder.ToLower();
        }

        fileName = Path.GetFileName(fileName);

        var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), ImageFolderName, folder);
        var fullPath = Path.Combine(imageFolderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public async Task SaveAsync(Stream imageStream, string fileName, string folder)
    {
        if (!string.IsNullOrWhiteSpace(folder))
        {
            folder = folder.ToLower();
        }

        var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), ImageFolderName, folder);

        if (!Directory.Exists(imageFolderPath))
        {
            Directory.CreateDirectory(imageFolderPath);
        }

        var fullPath = Path.Combine(imageFolderPath, fileName);

        using var stream = new FileStream(fullPath, FileMode.OpenOrCreate);

        await imageStream.CopyToAsync(stream);
    }
}
