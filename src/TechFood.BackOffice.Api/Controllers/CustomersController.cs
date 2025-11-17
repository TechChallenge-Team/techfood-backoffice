using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.BackOffice.Application.Customers.Commands.CreateCustomer;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;
using TechFood.BackOffice.Contracts.Customers;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.Lambda.Customers.Controllers;

[ApiController()]
[Route("v1/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Produces<CustomerDto>]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerRequest request)
    {
        var command = new CreateCustomerCommand(
            request.CPF,
            request.Name,
            request.Email);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{document}")]
    [Produces<CustomerDto>]
    public async Task<IActionResult> GetByDocumentAsync(string document)
    {
        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, document);

        var result = await _mediator.Send(query);

        return result != null
            ? Ok(result)
            : NotFound();
    }
}
