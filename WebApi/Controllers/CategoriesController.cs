using Application.Features.Categories.Commands;

using Common.Requests.Categories;
using Common.Wrappers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new category using the specified request data.
    /// </summary>
    /// <param name="request">The details of the category to create. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult containing a ResponseWrapper with the identifier of the newly created category if successful,
    /// or a ResponseWrapper with error details if the request is invalid.</returns>
    [HttpPost("create")]
    [EndpointName("CreateCategory")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateCategoryCommand { Request = request }, ct);
        return Ok(result);
    }
}
