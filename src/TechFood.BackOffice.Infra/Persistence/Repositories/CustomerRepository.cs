using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Repositories
{
    internal class CustomerRepository(BackOfficeContext dbContext) : ICustomerRepository
    {
        private readonly BackOfficeContext _dbContext = dbContext;

        public async Task<Guid> CreateAsync(Customer customer)
        {
            var entry = await _dbContext.AddAsync(customer);

            return entry.Entity.Id;
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByDocument(DocumentType type, string value)
        {
            return await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Document.Type == type && c.Document.Value == value);
        }

        public Task<Customer?> GetByDocumentAsync(DocumentType documentType, string documentValue)
        {
            return _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Document.Type == documentType && c.Document.Value == documentValue);
        }
    }
}
