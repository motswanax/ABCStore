using Application.Features.Categories.Commands;

using Common.Requests.Categories;
using Common.Wrappers;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ISender _mediator;

    public CategoriesController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <remarks>
    /// Returns <see cref="ResponseWrapper{T}"/> with the created category id in <c>Data</c>.
    /// If validation fails, returns <see cref="ResponseWrapper"/> with validation errors in <c>Messages</c>.
    /// </remarks>
    [HttpPost("create")]
    [EndpointName("CreateCategory")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateCategoryCommand { Request = request }, ct);
        return result.IsSuccessful ? Ok(result) : BadRequest(result);
    }
}
