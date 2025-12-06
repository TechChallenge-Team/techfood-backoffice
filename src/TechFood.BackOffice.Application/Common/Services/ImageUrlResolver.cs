using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TechFood.BackOffice.Application.Common.Services.Interfaces;

namespace TechFood.BackOffice.Application.Common.Services;

public partial class ImageUrlResolver : IImageUrlResolver
{
    private readonly IConfiguration _appConfiguration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    [GeneratedRegex(@"\s+")]
    private static partial Regex ImageNameRegex();

    public ImageUrlResolver(
        IConfiguration appConfiguration,
        IHttpContextAccessor httpContextAccessor)
    {
        _appConfiguration = appConfiguration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string BuildFilePath(string folderName, string imageFileName)
    {
        if (
            string.IsNullOrWhiteSpace(folderName) ||
            string.IsNullOrWhiteSpace(imageFileName))
        {
            throw new ApplicationException(Resources.Exceptions.ImageUrlResolver_FolderCannotBeNull);
        }

        var request = _httpContextAccessor.HttpContext!.Request;
        var basePath = request.PathBase.Value?.Trim('/');
        var baseUrl = _appConfiguration["TechFoodStaticImagesUrl"]?.Trim('/');

        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? -1,
            Path = $"{basePath}/{baseUrl}/{Uri.EscapeDataString(folderName)}/{Uri.EscapeDataString(imageFileName)}"
        };

        return uriBuilder.ToString();
    }

    public string CreateImageFileName(string categoryName, string contentType) =>
        $"{ImageNameRegex().Replace(categoryName.Trim(), "-")}-{DateTime.UtcNow:yyyyMMddHHmmss}.{contentType.Replace("image/", "")}";
}
