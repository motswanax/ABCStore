namespace Application.UnitTests.Features.Categories;

public static class CategoryParamData
{
        public static IEnumerable<object[]> GetValidCategoryData()
        {
            yield return new object[]
            {
                new Common.Requests.Categories.CreateCategoryRequest
                {
                    Name = "Electronics",
                    Description = "Devices and gadgets"
                }
            };
    
            yield return new object[]
            {
                new Common.Requests.Categories.CreateCategoryRequest
                {
                    Name = "Books",
                    Description = "All kinds of books"
                }
            };
    }
}
