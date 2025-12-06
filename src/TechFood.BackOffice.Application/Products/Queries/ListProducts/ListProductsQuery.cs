using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Queries.ListProducts;

public class ListProductsQuery : IRequest<List<ProductDto>>
{
    public class Handler(IProductQueryProvider queries) : IRequestHandler<ListProductsQuery, List<ProductDto>>
    {
        public Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
            => queries.GetAllAsync();
    }
}
