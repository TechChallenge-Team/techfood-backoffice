using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TechFood.BackOffice.Contracts.Common.Attributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxFileSize;
    public MaxFileSizeAttribute(long maxFileSize) => _maxFileSize = maxFileSize;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file || file.Length == 0)
        {
            return ValidationResult.Success;
        }

        return file.Length > _maxFileSize
            ? new ValidationResult($"O arquivo não pode exceder {_maxFileSize / (1024 * 1024)} MB.")
            : ValidationResult.Success;
    }
}
