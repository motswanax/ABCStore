using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;

using Common.Pagination;
using Common.Requests.Categories;
using Common.Responses.Categories;
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

    /// <summary>
    /// Retrieves the details of a category by its unique identifier.
    /// </summary>
    /// <remarks>Returns a 200 OK response with the category details if the category exists, or a 404 Not
    /// Found response if no category with the specified identifier is found.</remarks>
    /// <param name="categoryId">The unique identifier of the category to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult containing a ResponseWrapper with the category details if found; otherwise, a ResponseWrapper
    /// indicating that the category was not found.</returns>
    [HttpGet("get-by-id/{categoryId}")]
    [EndpointName("GetCategoryById")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery { CategoryId = categoryId }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all available categories.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An HTTP 200 response containing a wrapped list of category responses. The list will be empty if no categories
    /// are available.</returns>
    [HttpGet("all")]
    [EndpointName("GetAllCategories")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper<List<CategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoriesQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paginated list of categories that match the specified filter criteria.
    /// </summary>
    /// <param name="request">The filter parameters used to select and paginate the categories. May include search terms, page size, and page
    /// number.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An HTTP 200 response containing a paginated list of category data that matches the filter criteria.</returns>
    [HttpGet("get-paginated")]
    [EndpointName("GetPaginatedCategories")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(PaginatedResponse<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedCategories([FromQuery] CategoryFilterRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPaginatedCategoriesQuery { Request = request }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Updates the details of an existing category using the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the updated category information. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult containing a ResponseWrapper that indicates the result of the update operation. Returns a 200
    /// OK response if the update is successful, 400 Bad Request if the request is invalid, or 404 Not Found if the
    /// category does not exist.</returns>
    [HttpPut("update")]
    [EndpointName("UpdateCategory")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateCategoryCommand { Request = request }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the category with the specified identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to delete.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult containing a ResponseWrapper that indicates the result of the delete operation. Returns a 200
    /// OK response if the category is deleted successfully, or a 404 Not Found response if the category does not exist.</returns>
    [HttpDelete("delete/{categoryId}")]
    [EndpointName("DeleteCategory")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Tags("Categories")]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int categoryId, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand { CategoryId = categoryId }, ct);
        return Ok(result);
    }
}
