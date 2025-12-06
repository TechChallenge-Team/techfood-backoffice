using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Domain.Repositories;

namespace TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;

public class GetCustomerByDocumentHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByDocumentQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerByDocumentQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByDocumentAsync(request.DocumentType, request.DocumentValue);
        var result = new CustomerDto();

        if (customer is null)
        {
            return null;
        }

        return result.ConvertToDto(customer);
    }
}
