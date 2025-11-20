using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Repositories;

public class CategoryRepository(BackOfficeContext dbContext) : ICategoryRepository
{
    private readonly DbSet<Category> _categories = dbContext.Categories;

    public async Task<Guid> AddAsync(Category entity)
    {
        var result = await _categories.AddAsync(entity);

        return result.Entity.Id;
    }

    public async Task DeleteAsync(Category category)
        => await Task.FromResult(_categories.Remove(category));

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _categories.OrderBy(c => c.SortOrder).ToListAsync();

    public async Task<Category?> GetByIdAsync(Guid id)
        => await _categories.Where(x => x.Id == id).FirstOrDefaultAsync();

}
