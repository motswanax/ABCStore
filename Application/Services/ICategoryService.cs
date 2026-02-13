using Common.Pagination;
using Common.Requests.Categories;
using Common.Responses.Categories;

using Domain;

namespace Application.Services;

public interface ICategoryService
{
    Task<Category?> GetByIdAsync(int categoryId, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct);
    Task<PaginatedResponse<CategoryResponse>> GetPaginatedAsync(CategoryFilterRequest request, CancellationToken ct);
    Task<Category> CreateAsync(Category category, CancellationToken ct);
    Task<int> UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(Category category, CancellationToken ct);
}
