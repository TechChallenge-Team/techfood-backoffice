using System;
using System.Threading.Tasks;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.BackOffice.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Guid> CreateAsync(Customer customer);

    Task<Customer?> GetByDocumentAsync(DocumentType documentType, string documentValue);
}
