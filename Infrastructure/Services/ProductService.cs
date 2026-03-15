using Application.Services;

using Common.Pagination;
using Common.Requests.Products;
using Common.Responses.Products;

using Domain;

using Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    public async Task<Product> CreateAsync(Product request, CancellationToken ct)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
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

    public async Task<PaginatedResponse<ProductResponse>> GetPaginatedAsync(ProductFilterRequest request, CancellationToken ct)
    {
        var query = context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(x => x.Name.Contains(term) || x.Description != null && x.Description.Contains(term));
        }

        if (request.CategoryId > 0)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        query = request.SortBy switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "price" => request.SortDescending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
            _ => query.OrderBy(x => x.Id)
        };

        var total = await query.CountAsync(ct);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description ?? "N/A",
                Price = p.Price,
                CategoryId = p.CategoryId
            })
            .ToListAsync();

        return new PaginatedResponse<ProductResponse>
        {
            Data = data,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(total / (double)request.PageSize),
            TotalRecords = total
        };
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
