using Application.Exceptions;
using Application.Services;

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

        mockCategoryService.Setup(service => service.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category category, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(category, nameof(category));
                var existingCategory = mockCategories.FirstOrDefault(c => c.Id == category.Id) 
                ?? throw new NotFoundException([$"Category with id {category.Id} was not found."]);
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                return 1;
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
