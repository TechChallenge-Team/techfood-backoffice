using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);

    Task<IEnumerable<Product>> GetAllAsync();

    Task<Guid> AddAsync(Product entity);

    Task DeleteAsync(Product entity);
}
