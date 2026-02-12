namespace Domain;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
        
    // Navigation property
    public ICollection<Product> Products { get; set; } = [];
    /* Lessons learned from here. Better to use ICollection than IEnumerable for navigation properties to allow 
     * for lazy loading and change tracking in Entity Framework Core. Using IEnumerable can lead to issues with 
     * lazy loading and tracking changes to the collection, as it does not support adding or removing items directly.
     * When you use "= [];" with IEnumerable, it creates a new array that implements IEnumerable, but it does not 
     * allow for adding or removing items from the collection. But when you use "= [];" with ICollection, 
     * it creates a new List that implements ICollection, which allows for adding
     * and removing items directly.
     * */
}
