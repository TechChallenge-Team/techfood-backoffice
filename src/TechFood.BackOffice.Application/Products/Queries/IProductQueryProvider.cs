using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Queries;

public interface IProductQueryProvider
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(Guid id);
}
