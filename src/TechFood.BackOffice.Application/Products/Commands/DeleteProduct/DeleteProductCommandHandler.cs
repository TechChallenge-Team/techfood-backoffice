using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository repo, IImageStorageService imageStorage) : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repo.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new ApplicationException(Exceptions.Product_ProductNotFound);
        }

        await imageStorage.DeleteAsync(product.ImageFileName, nameof(Product));

        await repo.DeleteAsync(product);

        return Unit.Value;
    }
}
