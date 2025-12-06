using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Queries.ListCategories;

public class ListCategoriesQueryHandler(ICategoryQueryProvider queries) : IRequestHandler<ListCategoriesQuery, List<CategoryDto>>
{
    public Task<List<CategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        => queries.GetAllAsync();
}
