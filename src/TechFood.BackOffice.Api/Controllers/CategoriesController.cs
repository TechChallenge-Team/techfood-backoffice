using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.BackOffice.Application.Categories.Commands.CreateCategory;
using TechFood.BackOffice.Application.Categories.Commands.DeleteCategory;
using TechFood.BackOffice.Application.Categories.Commands.UpdateCategory;
using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries.GetCategory;
using TechFood.BackOffice.Application.Categories.Queries.ListCategories;
using TechFood.BackOffice.Contracts.Categories;

namespace TechFood.Api.Controllers;

[ApiController()]
[Route("v1/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Produces<CategoryDto>]
    public async Task<IActionResult> ListAsync()
    {
        var query = new ListCategoriesQuery();

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    [Produces<CategoryDto>]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var query = new GetCategoryQuery(id);

        var result = await _mediator.Send(query);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Produces<CategoryDto>]
    public async Task<IActionResult> CreateAsync([FromForm] CreateCategoryRequest request)
    {
        var imageFile = request.File;
        var command = new CreateCategoryCommand(
            request.Name,
            imageFile.OpenReadStream(),
            imageFile.ContentType);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [Produces<CategoryDto>]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] UpdateCategoryRequest request)
    {
        var imageFile = request.File;
        var command = new UpdateCategoryCommand(
            id,
            request.Name,
            imageFile?.OpenReadStream(),
            imageFile?.ContentType);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteCategoryCommand(id);

        await _mediator.Send(command);

        return NoContent();
    }
}
