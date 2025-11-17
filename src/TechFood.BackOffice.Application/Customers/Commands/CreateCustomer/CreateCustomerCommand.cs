using MediatR;
using TechFood.BackOffice.Application.Customers.Dto;

namespace TechFood.BackOffice.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(string CPF, string Name, string Email) : IRequest<CustomerDto>;
