using System.Net;
using System.Net.Http.Json;

using Common.Requests.Categories;
using Common.Wrappers;

using Shouldly;

namespace WebApi.UnitTests;

public class CategoriesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Create Category returns OK for valid request")]
    public async Task CreateCategory_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new CreateCategoryRequest
        {
            Name = "Books",
            Description = "All book categories"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Categories/create", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeTrue();
        body.Data.ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "Create Category returns BadRequest ResponseWrapper for invalid request")]
    public async Task CreateCategory_WithInvalidRequest_ReturnsBadRequestWithMessages()
    {
        // Arrange
        var request = new { Name = (string?)null, Description = (string?)null };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Categories/create", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeFalse();
        body.Messages.ShouldNotBeNull();
        body.Messages.Count.ShouldBeGreaterThan(0);
    }
}
