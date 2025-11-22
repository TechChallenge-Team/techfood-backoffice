using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Infra.Persistence.Contexts;

namespace TechFood.BackOffice.Infra.Persistence.Queries;

internal class CategoryQueryProvider(
    BackOfficeContext techFoodContext,
    IImageUrlResolver imageUrl) : ICategoryQueryProvider
{
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await techFoodContext.Categories
            .OrderBy(category => category.SortOrder)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageFileName 
            })
            .ToListAsync();

        foreach (var category in categories)
        {
            category.ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageUrl);
        }

        return categories;
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await techFoodContext.Categories
            .Where(x => x.Id == id)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageFileName
            })
            .FirstOrDefaultAsync();

        if (category != null)
        {
            category.ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageUrl);
        }

        return category;
    }
}
