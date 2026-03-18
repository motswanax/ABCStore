using Application.Exceptions;
using Application.Services;

using Domain;

using Moq;

namespace Application.UnitTests.Features.Products;

public static class MockProductService
{
    public static Mock<IProductService> GetProductServiceMocks()
    {
        // Database mock
        var mockProducts = GetMockProducts();
        // Create mock for IProductService methods
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product product, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(product, nameof(product));
                product.Id = mockProducts.Max(p => p.Id) + 1;
                mockProducts.Add(product);
                return product;
            });

        mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int productId, CancellationToken ct) =>
                mockProducts.FirstOrDefault(p => p.Id == productId));

        mockProductService.Setup(service => service.GetProductsByCategoryIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int categoryId, CancellationToken ct) =>
                mockProducts.Where(p => p.CategoryId == categoryId).ToList());

        mockProductService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockProducts);

        mockProductService.Setup(service => service.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product product, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(product, nameof(product));
                var existingProduct = mockProducts.FirstOrDefault(p => p.Id == product.Id)
                ?? throw new NotFoundException([$"Product with ID {product.Id} not found."]);
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                return existingProduct.Id; // Updated
            });

        mockProductService.Setup(service => service.DeleteAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns((Product product, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(product, nameof(product));
                var existingProduct = mockProducts.FirstOrDefault(p => p.Id == product.Id)
                ?? throw new NotFoundException([$"Product with ID {product.Id} not found."]);
                mockProducts.Remove(existingProduct);
                return Task.CompletedTask;
            });

        return mockProductService;
    }

    public static Mock<IProductService> GetEmptyProductServiceMocks()
    {
        // Database mock
        var mockProducts = GetEmptyMockProducts();
        // Create mock for IProductService methods
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product product, CancellationToken ct) =>
            {
                ArgumentNullException.ThrowIfNull(product, nameof(product));
                product.Id = (mockProducts.Count == 0 ? 0 : mockProducts.Max(p => p.Id)) + 1;
                mockProducts.Add(product);
                return product;
            });

        mockProductService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockProducts);

        mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int productId, CancellationToken ct) =>
                mockProducts.FirstOrDefault(p => p.Id == productId));

        mockProductService.Setup(service => service.GetProductsByCategoryIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int categoryId, CancellationToken ct) =>
                mockProducts.Where(p => p.CategoryId == categoryId).ToList());

        return mockProductService;
    }

    private static List<Product> GetMockProducts()
    {
        return 
        [
            new() { Id = 1, CategoryId = 1, Name = "Product 1", Description = "Description for Product 1", Price = 9.99m },
            new() { Id = 2, CategoryId = 1, Name = "Product 2", Description = "Description for Product 2", Price = 25.99m },
            new() { Id = 3, CategoryId = 1, Name = "Product 3", Description = "Description for Product 3", Price = 35.99m },
        ];
    }

    private static List<Product> GetEmptyMockProducts()
    {
        return [];
    }
}
