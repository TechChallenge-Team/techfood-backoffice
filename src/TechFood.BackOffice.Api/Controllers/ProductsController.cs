using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.BackOffice.Application.Products.Commands.CreateProduct;
using TechFood.BackOffice.Application.Products.Commands.DeleteProduct;
using TechFood.BackOffice.Application.Products.Commands.SetProductOutOfStock;
using TechFood.BackOffice.Application.Products.Commands.UpdateProduct;
using TechFood.BackOffice.Application.Products.Queries.GetProduct;
using TechFood.BackOffice.Application.Products.Queries.ListProducts;
using TechFood.BackOffice.Contracts.Products;

namespace TechFood.Api.Controllers;

[ApiController()]
[Route("v1/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new ListProductsQuery();

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var query = new GetProductQuery(id);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUpdateRequest request)
    {
        var imageFile = request.File;
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.CategoryId,
            imageFile.OpenReadStream(),
            imageFile.ContentType,
            request.Price);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var imageFile = request.File;
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.CategoryId,
            request.Price,
            imageFile.OpenReadStream(),
            imageFile.ContentType);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/outOfStock")]
    public async Task<IActionResult> PatchOutOfStockAsync(Guid id, bool request)
    {
        var command = new SetProductOutOfStockCommand(id, request);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteProductCommand(id);

        await _mediator.Send(command);

        return NoContent();
    }
}
