using System;
using System.IO;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    Stream? ImageFile,
    string? ImageContentType) : IRequest<CategoryDto>;
