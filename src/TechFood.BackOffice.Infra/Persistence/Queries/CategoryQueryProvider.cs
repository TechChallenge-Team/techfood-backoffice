using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Queries;

internal class CategoryQueryProvider(
    BackOfficeContext techFoodContext,
    IImageUrlResolver imageUrl) : ICategoryQueryProvider
{
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await techFoodContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.SortOrder)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageFileName)
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        return await techFoodContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = imageUrl.BuildFilePath(nameof(Category).ToLower(), category.ImageFileName)
            })
            .FirstOrDefaultAsync();
    }
}
