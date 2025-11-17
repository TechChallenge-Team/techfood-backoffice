using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Queries;

public interface ICategoryQueryProvider
{
    Task<List<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(Guid id);
}
