using Domain;

namespace Application.Services;

public interface IProductService
{
    Task<Product> CreateAsync(Product request, CancellationToken ct); 
    Task<Product> GetByIdAsync(int productId, CancellationToken ct); 
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct); 
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken ct); 
    Task<int> UpdateAsync(Product request, CancellationToken ct); 
    Task DeleteAsync(int productId, CancellationToken ct);
}
