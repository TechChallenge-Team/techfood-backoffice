using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Products.Commands.SetProductOutOfStock;

public class SetProductOutOfStockCommandHandler(IProductRepository repo, IImageUrlResolver imageUrl) : IRequestHandler<SetProductOutOfStockCommand, ProductDto>
{
    public async Task<ProductDto> Handle(SetProductOutOfStockCommand request, CancellationToken cancellationToken)
    {
        var product = await repo.GetByIdAsync(request.Id);
        if (product is null)
        {
            throw new ApplicationException(Exceptions.Product_ProductNotFound);
        }

        product.SetOutOfStock(request.OutOfStock);

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
