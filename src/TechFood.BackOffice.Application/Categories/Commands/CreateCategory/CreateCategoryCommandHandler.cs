using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Repositories;

namespace TechFood.BackOffice.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
        ICategoryRepository repo,
        IImageStorageService imageStore,
        IImageUrlResolver imageUrl)
            : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var imageFileName = imageUrl.CreateImageFileName(request.Name, request.ImageContentType);

        var category = new BackOffice.Domain.Entities.Category(request.Name, imageFileName, 0);

        await imageStore.SaveAsync(request.ImageFile,
                                   imageFileName,
                                   nameof(Categories));

        await repo.AddAsync(category);

        return new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = imageUrl.BuildFilePath(nameof(Categories).ToLower(), imageFileName)
        };
    }
}
