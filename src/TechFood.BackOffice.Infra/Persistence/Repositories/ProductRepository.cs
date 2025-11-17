using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Repositories;

public class ProductRepository(BackOfficeContext dbContext) : IProductRepository
{
    private readonly DbSet<Product> _products = dbContext.Products;

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _products.Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Guid> AddAsync(Product product)
    {
        var session = await _products.AddAsync(product);

        return session.Entity.Id;
    }

    public async Task DeleteAsync(Product product)
        => await Task.FromResult(_products.Remove(product));
}
