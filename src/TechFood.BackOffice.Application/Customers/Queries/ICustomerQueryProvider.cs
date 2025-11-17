using System.Threading.Tasks;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.BackOffice.Application.Customers.Queries;

public interface ICustomerQueryProvider
{
    Task<CustomerDto?> GetByDocumentAsync(DocumentType documentType, string document);
}
