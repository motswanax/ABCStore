using Application.Services;

using Domain;

using Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    public async Task<Category> CreateAsync(Category category, CancellationToken ct)
    {
        await context.Categories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);
        return category;
    }

    public async Task DeleteAsync(Category category, CancellationToken ct)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct)
    {
        return await context.Categories.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(int categoryId, CancellationToken ct)
    {
        return await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoryId, ct);
    }

    public async Task<int> UpdateAsync(Category category, CancellationToken ct)
    {
        context.Entry(category).State = EntityState.Modified; // Mark the entity as modified
        await context.SaveChangesAsync(ct);
        return category.Id;
    }
}
