using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(ICustomerRepository repo) : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var document = new Document(DocumentType.CPF, request.CPF);

        var cpfExists = await repo.GetByDocumentAsync(document.Type, document.Value);
        if (cpfExists != null)
        {
            throw new ApplicationException(Exceptions.Customer_CpfAlreadyExists);
        }

        var customer = new BackOffice.Domain.Entities.Customer(
            new Name(request.Name),
            new Email(request.Email),
            document,
            null
        );

        var id = await repo.CreateAsync(customer);

        return new CustomerDto()
        {
            Id = id,
            DocumentType = customer.Document.Type,
            DocumentValue = customer.Document.Value,
            Name = customer.Name.FullName,
            Email = customer.Email.Address,
            Phone = customer.Phone?.Number,
        };
    }
}
