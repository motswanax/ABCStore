using Domain;

namespace Application.Services;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(int categoryId, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct);
    Task<Category> CreateAsync(Category category, CancellationToken ct);
    Task<int> UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(Category category, CancellationToken ct);
}
