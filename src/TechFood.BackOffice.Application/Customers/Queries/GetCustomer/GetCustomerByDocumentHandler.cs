using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Customers.Dto;

namespace TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;

public class GetCustomerByDocumentHandler(ICustomerQueryProvider queries) : IRequestHandler<GetCustomerByDocumentQuery, CustomerDto?>
{
    public Task<CustomerDto?> Handle(GetCustomerByDocumentQuery request, CancellationToken cancellationToken)
        => queries.GetByDocumentAsync(request.DocumentType, request.DocumentValue);
}
