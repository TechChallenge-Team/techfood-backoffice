using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Menu.Dto;
using TechFood.BackOffice.Application.Menu.Queries;
using TechFood.BackOffice.Domain.Entities;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Queries;

internal class MenuQueryProvider(BackOfficeContext techFoodContext, IImageUrlResolver imageUrl) : IMenuQueryProvider
{
    public async Task<MenuDto> GetAsync()
    {
        var categories = await techFoodContext.Categories
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageFileName),
                SortOrder = category.SortOrder,
                Products = techFoodContext.Products
                    .Where(p => p.CategoryId == category.Id)
                    .Select(product => new ProductDto
                    {
                        Id = product.Id,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        ImageUrl = imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName)
                    }).ToList()
            }).ToListAsync();

        return new()
        {
            Categories = categories
        };
    }
}
