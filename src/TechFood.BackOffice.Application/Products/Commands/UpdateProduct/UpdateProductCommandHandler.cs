using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IImageUrlResolver imageUrl,
    IImageStorageService imageStore)
        : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id);
        if (product is null)
        {
            throw new ApplicationException(Exceptions.Product_ProductNotFound);
        }

        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
        {
            throw new ApplicationException(Exceptions.Product_CaregoryNotFound);
        }

        var imageFileName = product.ImageFileName;

        if (request.ImageFile != null)
        {
            imageFileName = imageUrl.CreateImageFileName(request.Name, request.ImageContentType!);

            await imageStore.SaveAsync(request.ImageFile,
                                       imageFileName,
                                       nameof(Product));

            await imageStore.DeleteAsync(product.ImageFileName, nameof(Product));
        }

        product!.Update(
            request.Name,
            request.Description,
            imageFileName,
            request.Price,
            category.Id);

        return new ProductDto(
           product.Id,
           product.Name,
           product.Description,
           product.CategoryId,
           product.OutOfStock,
           imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName),
           product.Price
       );
    }
}
