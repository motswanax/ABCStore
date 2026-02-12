using Domain;

namespace Application.Services;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(int categoryId);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> CreateAsync(Category category);
    Task<int> UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}
