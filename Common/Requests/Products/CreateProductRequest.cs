using Common.Pipelines;

namespace Common.Requests.Products;

public class CreateProductRequest : IValidateMe
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}
