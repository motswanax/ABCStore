using Common.Pipelines;

namespace Common.Requests.Categories;

public class CreateCategoryRequest : IValidateMe
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
