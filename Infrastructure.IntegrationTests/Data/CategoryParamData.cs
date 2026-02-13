using Domain;

namespace Infrastructure.IntegrationTests.Data;

/// <summary>
/// Provides test case data for category-related integration tests.
/// </summary>
/// <remarks>
/// This type is intended to be used as a source for parameterized tests (for example, xUnit <c>[MemberData]</c>).
/// Each yielded item is an <see cref="object"/> array whose first element is a <see cref="Category"/> instance.
/// </remarks>
public static class CategoryParamData
{
    /// <summary>
    /// Returns valid <see cref="Category"/> instances to be used when testing category creation scenarios.
    /// </summary>
    /// <returns>
    /// A sequence of <see cref="IEnumerable{T}"/> items where each item is an <see cref="object"/> array containing
    /// a single <see cref="Category"/> to be used as input for creation tests.
    /// </returns>
    public static IEnumerable<object[]> GetValidCategoriesForCreation()
    {
        yield return new object[]
        {
            new Category
            {
                Id = 3,
                Name = "Category 3",
                Description = "Description for Category 3"
            }
        };
        yield return new object[] 
        {
            new Category
            {
                Id = 4,
                Name = "Category 4",
                Description = "Description for Category 4"
            }
        };
    }
}
