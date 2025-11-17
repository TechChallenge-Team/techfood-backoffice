using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IImageUrlResolver imageUrl,
    IImageStorageService imageStore)
        : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
        {
            throw new NotFoundException(Exceptions.Product_CaregoryNotFound);
        }

        var imageFileName = imageUrl.CreateImageFileName(request.Name, request.ImageContentType);

        await imageStore.SaveAsync(
                request.ImageFile,
                imageFileName, nameof(Product));

        var product = new Product(
            request.Name,
            request.Description,
            request.CategoryId,
            imageFileName,
            request.Price);

        await productRepository.AddAsync(product);

        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.CategoryId,
            product.OutOfStock,
            imageUrl.BuildFilePath(nameof(Product).ToLower(), imageFileName),
            product.Price
            );
    }
}
