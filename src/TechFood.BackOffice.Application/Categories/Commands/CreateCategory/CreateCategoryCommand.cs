using System.IO;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    Stream ImageFile,
    string ImageContentType) : IRequest<CategoryDto>;
