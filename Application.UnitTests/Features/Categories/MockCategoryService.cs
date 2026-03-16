using Application.Exceptions;
using Application.Services;

using Common.Pagination;
using Common.Requests.Categories;
using Common.Responses.Categories;

using Domain;

using Moq;

namespace Application.UnitTests.Features.Categories;

public static class MockCategoryService
{
    public static Mock<ICategoryService> GetCategoryServiceMocks()
    {
        // Database mock
        var mockCategories = GetMockCategories();

        // Create mock for ICategoryService methods
        var mockCategoryService = new Mock<ICategoryService>();
        mockCategoryService.Setup(service => service.CreateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category category, CancellationToken ct) =>
            {
                category.Id = mockCategories.Max(c => c.Id) + 1;
                mockCategories.Add(category);
                return category;
            });

        mockCategoryService.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int categoryId, CancellationToken ct) =>
                mockCategories.FirstOrDefault(c => c.Id == categoryId));

        mockCategoryService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCategories);

        mockCategoryService.Setup(service => service.GetPaginatedAsync(It.IsAny<CategoryFilterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryFilterRequest request, CancellationToken ct) =>
            {
                IEnumerable<Category> query = mockCategories;

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.Trim();
                    query = query.Where(x => x.Name.Contains(term) || x.Description.Contains(term));
                }

                query = request.SortBy switch
                {
                    "name" => request.SortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    _ => query.OrderBy(x => x.Id)
                };

                var total = query.Count();
                var data = query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new CategoryResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                return new PaginatedResponse<CategoryResponse>
                {
                    TotalRecords = total,
                    Data = data,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
                };
            });

        mockCategoryService.Setup(service => service.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category category, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(category, nameof(category));
                var existingCategory = mockCategories.FirstOrDefault(c => c.Id == category.Id) 
                ?? throw new NotFoundException([$"Category with id {category.Id} was not found."]);
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                return existingCategory.Id;
            });

        mockCategoryService.Setup(service => service.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns((Category category, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(category, nameof(category));
                var existingCategory = mockCategories.FirstOrDefault(c => c.Id == category.Id)
                ?? throw new NotFoundException([$"Category with id {category.Id} was not found."]);
                mockCategories.Remove(existingCategory);
                return Task.CompletedTask;
            });

        return mockCategoryService;
    }

    private static List<Category> GetMockCategories()
    {
        return
        [
            new() { Id = 1, Name = "Category 1", Description = "Description for Category 1" },
            new() { Id = 2, Name = "Category 2", Description = "Description for Category 2" },
            new() { Id = 3, Name = "Category 3", Description = "Description for Category 3" },
            new() { Id = 4, Name = "Category 4", Description = "Description for Category 4" },
            new() { Id = 5, Name = "Category 5", Description = "Description for Category 5" }
        ];
    }
}
