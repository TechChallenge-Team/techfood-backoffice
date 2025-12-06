using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<Guid> AddAsync(Category category);

    Task DeleteAsync(Category category);
}
