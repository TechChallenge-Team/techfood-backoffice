using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.BackOffice.Application.Menu.Queries.GetMenu;

namespace TechFood.Api.Controllers;

[ApiController()]
[Route("v1/[controller]")]
public class MenuController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var query = new GetMenuQuery();

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
