using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Infra.Persistence.Contexts;

namespace TechFood.BackOffice.Infra.Persistence.Queries;

internal class ProductQueryProvider(BackOfficeContext techFoodContext, IImageUrlResolver imageUrl) : IProductQueryProvider
{
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var productsData = await techFoodContext.Products
            .Select(product => new
            {
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.OutOfStock,
                product.ImageFileName,
                product.Price
            })
            .ToListAsync();

        return productsData.Select(product => new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.CategoryId,
            product.OutOfStock,
            imageUrl.BuildFilePath(nameof(Product).ToLower(), product.ImageFileName),
            product.Price))
            .ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var productData = await techFoodContext.Products
            .Where(product => product.Id == id)
            .Select(product => new
            {
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.OutOfStock,
                product.ImageFileName,
                product.Price
            })
            .FirstOrDefaultAsync();

        if (productData == null)
            return null;

        return new ProductDto(
            productData.Id,
            productData.Name,
            productData.Description,
            productData.CategoryId,
            productData.OutOfStock,
            imageUrl.BuildFilePath(nameof(Product).ToLower(), productData.ImageFileName),
            productData.Price);
    }
}
