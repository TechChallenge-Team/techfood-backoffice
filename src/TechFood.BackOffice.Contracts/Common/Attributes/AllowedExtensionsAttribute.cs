using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TechFood.BackOffice.Contracts.Common.Attributes;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(params string[] extensions) => _extensions = extensions;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is not IFormFile file || file.Length == 0)
        {
            return ValidationResult.Success;
        }

        var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        return !_extensions.Contains(ext)
            ? new ValidationResult($"Extensão inválida. Permitido: {string.Join(", ", _extensions)}")
            : ValidationResult.Success;
    }
}
