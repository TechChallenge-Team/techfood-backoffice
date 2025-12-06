using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Infra.Persistence.Contexts;

namespace TechFood.BackOffice.Infra.Persistence.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly BackOfficeContext _context;

        public CustomerRepository(BackOfficeContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByDocument(DocumentType type, string value)
        {
            return await GetByDocumentAsync(type, value);
        }

        public async Task<Customer?> GetByDocumentAsync(DocumentType documentType, string documentValue)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Document.Type == documentType && c.Document.Value == documentValue);
        }
    }
}
