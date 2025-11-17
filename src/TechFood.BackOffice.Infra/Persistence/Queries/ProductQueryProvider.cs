using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Entities;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Queries;

internal class ProductQueryProvider(BackOfficeContext techFoodContext, IImageUrlResolver imageUrl) : IProductQueryProvider
{
    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await techFoodContext.Products
            .AsNoTracking()
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.OutOfStock,
                imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName),
                product.Price))
            .ToListAsync();
    }

    public Task<ProductDto?> GetByIdAsync(Guid id)
    {
        return techFoodContext.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.OutOfStock,
                imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName),
                product.Price))
            .FirstOrDefaultAsync();
    }
}
