using Domain;

namespace Infrastructure.IntegrationTests.Data;

public static class ProductParamData
{
    public static IEnumerable<object[]> GetProductsForCreation()
    {
        yield return new object[]
        {
            new Product
            {
                Name = "New Product 5",
                Description = "Description for New Product 1",
                Price = 19.99m,
                CategoryId = 1
            }
        };
        yield return new object[]
        {
            new Product
            {
                Id = 6,
                Name = "New Product 6",
                Description = "Description for New Product 2",
                Price = 29.99m,
                CategoryId = 2
            }
        };
        yield return new object[]
        {
            new Product
            {
                Name = "New Product 7",
                Description = "Description for New Product 3",
                Price = 39.99m,
                CategoryId = 2
            }
        };
    }

    public static IEnumerable<object[]> GetProductsForUpdating()
    {
        yield return new object[]
        {
            new Product
            {
                Id = 1,
                Name = "Updated Product 1",
                Description = "Updated Description for Product 1",
                Price = 19.99m,
                CategoryId = 1
            }
        };
        yield return new object[]
        {
            new Product
            {
                Id = 2,
                Name = "Updated Product 2",
                Description = "Updated Description for Product 2",
                Price = 29.99m,
                CategoryId = 2
            }
        };
    }
}
