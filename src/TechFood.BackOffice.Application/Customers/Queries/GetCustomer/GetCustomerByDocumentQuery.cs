using MediatR;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;

public record GetCustomerByDocumentQuery(DocumentType DocumentType, string DocumentValue) : IRequest<CustomerDto?>;
