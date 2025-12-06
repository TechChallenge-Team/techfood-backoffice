using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechFood.BackOffice.Contracts.Common.Attributes;

namespace TechFood.BackOffice.Contracts.Categories;

public record CreateCategoryRequest(
    [Required] string Name,
    [Required, MaxFileSize(5 * 1024 * 1024), AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")] IFormFile File);
