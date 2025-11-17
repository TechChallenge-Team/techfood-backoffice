using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechFood.BackOffice.Contracts.Common.Attributes;

namespace TechFood.BackOffice.Contracts.Products;

public record CreateUpdateRequest(
    [Required] string Name,
    [Required] string Description,
    [Required] Guid CategoryId,
    [Required] decimal Price,
    [Required, MaxFileSize(5 * 1024 * 1024), AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")] IFormFile File);
