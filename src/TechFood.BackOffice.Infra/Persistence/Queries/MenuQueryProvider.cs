using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
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
        // First, get categories with their basic data
        var categoriesData = await techFoodContext.Categories
            .OrderBy(c => c.SortOrder)
            .Select(category => new
            {
                category.Id,
                category.Name,
                category.ImageFileName,
                category.SortOrder
            })
            .ToListAsync();

        // Then get all products
        var productsData = await techFoodContext.Products
            .Select(product => new
            {
                product.Id,
                product.CategoryId,
                product.Name,
                product.Description,
                product.Price,
                product.ImageFileName
            })
            .ToListAsync();

        // Build the final result with image URLs
        var categories = categoriesData.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageFileName),
            SortOrder = category.SortOrder,
            Products = productsData
                .Where(p => p.CategoryId == category.Id)
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName)
                })
                .ToList()
        }).ToList();

        return new()
        {
            Categories = categories
        };
    }
}
