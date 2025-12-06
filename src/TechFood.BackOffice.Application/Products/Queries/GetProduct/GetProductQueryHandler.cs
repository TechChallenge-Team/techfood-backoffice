using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler(IProductQueryProvider queries) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        => queries.GetByIdAsync(request.Id);
}
