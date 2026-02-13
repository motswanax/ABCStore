using Application.Services;

using Common.Pagination;
using Common.Requests.Categories;
using Common.Responses.Categories;

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

    public async Task<PaginatedResponse<CategoryResponse>> GetPaginatedAsync(CategoryFilterRequest request, CancellationToken ct)
    {
        var query = context.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(x => x.Name.Contains(term) || x.Description.Contains(term));
        }

        query = request.SortBy switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ => query.OrderBy(x => x.Id)
        };

        var total = await query.CountAsync(ct);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new CategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync(ct);

        return new PaginatedResponse<CategoryResponse>
        {
            TotalRecords = total,
            Data = data,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
        };
    }

    public async Task<int> UpdateAsync(Category category, CancellationToken ct)
    {
        context.Entry(category).State = EntityState.Modified; // Mark the entity as modified
        await context.SaveChangesAsync(ct);
        return category.Id;
    }
}
