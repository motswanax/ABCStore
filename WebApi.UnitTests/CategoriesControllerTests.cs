using System.Net;
using System.Net.Http.Json;

using Common.Requests.Categories;
using Common.Wrappers;

using Infrastructure.Contexts;

using Microsoft.Extensions.DependencyInjection;

using Domain;

using Shouldly;

namespace WebApi.UnitTests;

public class CategoriesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CategoriesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "TC1: Create Category returns OK for valid request")]
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

    [Fact(DisplayName = "TC2: Create Category returns BadRequest ResponseWrapper for invalid request")]
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

    [Fact(DisplayName = "TC3: Update Category returns OK for valid request")]
    public async Task UpdateCategory_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var create = new CreateCategoryRequest
        {
            Name = "Electronics",
            Description = "All electronics"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categories/create", create);
        createResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var createdBody = await createResponse.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        createdBody.ShouldNotBeNull();
        createdBody.Data.ShouldBeGreaterThan(0);

        var update = new UpdateCategoryRequest
        {
            Id = createdBody.Data!,
            Name = "Electronics Updated",
            Description = "All electronics (updated)"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Categories/update", update);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeTrue();
        body.Data.ShouldBe(update.Id);
    }

    [Fact(DisplayName = "TC4: Update Category returns OK but unsuccessful wrapper when category not found")]
    public async Task UpdateCategory_WithNonExistingId_ReturnsOkWithFailureWrapper()
    {
        // Arrange
        var update = new UpdateCategoryRequest
        {
            Id = 99999,
            Name = "Does not matter",
            Description = "Does not matter"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Categories/update", update);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeFalse();
        body.Messages.ShouldContain("Category not found.");
    }

    [Fact(DisplayName = "TC5: Update Category returns BadRequest ResponseWrapper for invalid request")]
    public async Task UpdateCategory_WithInvalidRequest_ReturnsBadRequestWithMessages()
    {
        // Arrange
        var request = new { Id = 0, Name = (string?)null, Description = (string?)null };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Categories/update", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeFalse();
        body.Messages.ShouldNotBeNull();
        body.Messages.Count.ShouldBeGreaterThan(0);
    }
}
