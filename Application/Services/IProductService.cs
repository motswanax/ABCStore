using Common.Pagination;
using Common.Requests.Products;
using Common.Responses.Products;

using Domain;

namespace Application.Services;

public interface IProductService
{
    Task<Product> CreateAsync(Product request, CancellationToken ct); 
    Task<Product?> GetByIdAsync(int productId, CancellationToken ct); 
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct);
    Task<PaginatedResponse<ProductResponse>> GetPaginatedAsync(ProductFilterRequest request, CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken ct); 
    Task<int> UpdateAsync(Product request, CancellationToken ct); 
    Task DeleteAsync(Product product, CancellationToken ct);
}
