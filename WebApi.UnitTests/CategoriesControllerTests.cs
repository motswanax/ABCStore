using System.Net;
using System.Net.Http.Json;

using Common.Requests.Categories;
using Common.Responses.Categories;
using Common.Wrappers;

using Common.Pagination;

using Shouldly;

namespace WebApi.UnitTests;

public class CategoriesControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

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

    [Fact(DisplayName = "TC6: Delete Category returns OK for valid id")]
    public async Task DeleteCategory_WithValidId_ReturnsOk()
    {
        // Arrange
        var create = new CreateCategoryRequest
        {
            Name = "Clothing",
            Description = "All clothing"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categories/create", create);
        createResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var createdBody = await createResponse.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        createdBody.ShouldNotBeNull();
        createdBody.Data.ShouldBeGreaterThan(0);

        // Act
        var response = await _client.DeleteAsync($"/api/Categories/delete/{createdBody.Data}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeTrue();
        body.Data.ShouldBe(createdBody.Data);
    }

    [Fact(DisplayName = "TC7: Delete Category returns OK but unsuccessful wrapper when category not found")]
    public async Task DeleteCategory_WithNonExistingId_ReturnsOkWithFailureWrapper()
    {
        // Act
        var response = await _client.DeleteAsync("/api/Categories/delete/99999");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeFalse();
        body.Messages.ShouldContain("Category not found.");
    }

    [Fact(DisplayName = "TC8: Get Category by id returns OK for existing id")]
    public async Task GetCategoryById_WithExistingId_ReturnsOk()
    {
        // Arrange
        var create = new CreateCategoryRequest
        {
            Name = "Sports",
            Description = "All sports"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categories/create", create);
        createResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var createdBody = await createResponse.Content.ReadFromJsonAsync<ResponseWrapper<int>>();
        createdBody.ShouldNotBeNull();
        createdBody.Data.ShouldBeGreaterThan(0);

        // Act
        var response = await _client.GetAsync($"/api/Categories/get-by-id/{createdBody.Data}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<CategoryResponse>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeTrue();
        body.Data.ShouldNotBeNull();
        body.Data.Id.ShouldBe(createdBody.Data);
    }

    [Fact(DisplayName = "TC9: Get Category by id returns OK but unsuccessful wrapper when not found")]
    public async Task GetCategoryById_WithNonExistingId_ReturnsOkWithFailureWrapper()
    {
        // Act
        var response = await _client.GetAsync("/api/Categories/get-by-id/99999");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<CategoryResponse>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeFalse();
        body.Messages.ShouldContain("Category not found.");
    }

    [Fact(DisplayName = "TC10: Get All Categories returns OK and list")]
    public async Task GetAllCategories_ReturnsOk()
    {
        // Arrange
        await _client.PostAsJsonAsync("/api/Categories/create", new CreateCategoryRequest { Name = "Cat A", Description = "Desc A" });
        await _client.PostAsJsonAsync("/api/Categories/create", new CreateCategoryRequest { Name = "Cat B", Description = "Desc B" });

        // Act
        var response = await _client.GetAsync("/api/Categories/all");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ResponseWrapper<IEnumerable<CategoryResponse>>>();
        body.ShouldNotBeNull();
        body.IsSuccessful.ShouldBeTrue();
        body.Data.ShouldNotBeNull();
        body.Data.Count().ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact(DisplayName = "TC11: Get Paginated Categories returns OK and pagination metadata")]
    public async Task GetPaginatedCategories_ReturnsOk()
    {
        // Arrange
        await _client.PostAsJsonAsync("/api/Categories/create", new CreateCategoryRequest { Name = "Pag1", Description = "D1" });
        await _client.PostAsJsonAsync("/api/Categories/create", new CreateCategoryRequest { Name = "Pag2", Description = "D2" });
        await _client.PostAsJsonAsync("/api/Categories/create", new CreateCategoryRequest { Name = "Pag3", Description = "D3" });

        // Act
        var response = await _client.GetAsync("/api/Categories/get-paginated?pageNumber=1&pageSize=2&sortBy=name&sortDescending=false");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PaginatedResponse<CategoryResponse>>();
        body.ShouldNotBeNull();
        body.PageNumber.ShouldBe(1);
        body.PageSize.ShouldBe(2);
        body.Data.ShouldNotBeNull();
        body.Data.Count.ShouldBeLessThanOrEqualTo(2);
        body.TotalRecords.ShouldBeGreaterThan(0);
        body.TotalPages.ShouldBeGreaterThan(0);
    }
}
