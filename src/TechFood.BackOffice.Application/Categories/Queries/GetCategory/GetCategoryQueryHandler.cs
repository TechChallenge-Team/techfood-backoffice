using System.Threading.Tasks;
using System.Threading;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler(ICategoryQueryProvider queries) : IRequestHandler<GetCategoryQuery, CategoryDto?>
{
    public Task<CategoryDto?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        => queries.GetByIdAsync(request.Id);
}
