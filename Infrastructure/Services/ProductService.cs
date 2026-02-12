using Application.Services;

using Domain;

using Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    public async Task<Product> CreateAsync(Product request, CancellationToken ct)
    {
        await context.Products.AddAsync(request);
        await context.SaveChangesAsync(ct); 
        return request;
    }

    public Task DeleteAsync(Product product, CancellationToken ct)
    {
        context.Products.Remove(product); 
        return context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        return await context.Products.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken ct)
    {
        return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId, ct);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return await context.Products.AsNoTracking().Where(p => p.CategoryId == categoryId).ToListAsync(ct);
    }

    public async Task<int> UpdateAsync(Product request, CancellationToken ct)
    {
        context.Entry(request).State = EntityState.Modified; // Mark the entity as modified
        await context.SaveChangesAsync(ct);
        return request.Id;
    }
}
