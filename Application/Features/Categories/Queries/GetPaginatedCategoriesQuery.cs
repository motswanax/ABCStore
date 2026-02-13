using Application.Services;

using Common.Pagination;
using Common.Requests.Categories;
using Common.Responses.Categories;

using MediatR;

namespace Application.Features.Categories.Queries;

public class GetPaginatedCategoriesQuery : IRequest<PaginatedResponse<CategoryResponse>>
{
    public CategoryFilterRequest Request { get; set; } = new();
}

public class GetPaginatedCategoriesQueryHandler(ICategoryService categoryService) 
    : IRequestHandler<GetPaginatedCategoriesQuery, PaginatedResponse<CategoryResponse>>
{
    public async Task<PaginatedResponse<CategoryResponse>> Handle(GetPaginatedCategoriesQuery query, CancellationToken ct)
    {
        return await categoryService.GetPaginatedAsync(query.Request, ct);
    }
}