namespace Domain;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation property
    public IEnumerable<Product> Products { get; set; } = [];
}
