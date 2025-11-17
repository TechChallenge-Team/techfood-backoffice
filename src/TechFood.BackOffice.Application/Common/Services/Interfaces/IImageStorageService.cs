using System.IO;
using System.Threading.Tasks;

namespace TechFood.BackOffice.Application.Common.Services.Interfaces;

public interface IImageStorageService
{
    Task SaveAsync(Stream imageStream, string fileName, string folder);

    Task DeleteAsync(string fileName, string folder);
}
