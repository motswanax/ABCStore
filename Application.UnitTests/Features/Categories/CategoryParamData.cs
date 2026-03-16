using Common.Requests.Categories;

namespace Application.UnitTests.Features.Categories;

public static class CategoryParamData
{
    public static IEnumerable<object[]> GetValidCategoryData()
    {
        yield return new object[]
        {
                new CreateCategoryRequest
                {
                    Name = "Electronics",
                    Description = "Devices and gadgets"
                }
        };

        yield return new object[]
        {
                new CreateCategoryRequest
                {
                    Name = "Books",
                    Description = "All kinds of books"
                }
        };
    }

    public static IEnumerable<object[]> GetValidCategoryDataForUpdate()
    {
        yield return new object[]
        {
                new UpdateCategoryRequest
                {
                    Id = 1,
                    Name = "Electronics - Updated",
                    Description = "Devices and gadgets - Updated"
                }
        };

        yield return new object[]
        {
                new UpdateCategoryRequest
                {
                    Id= 2,
                    Name = "Books - Updated",
                    Description = "All kinds of books - Updated"
                }
        };
    }
}
