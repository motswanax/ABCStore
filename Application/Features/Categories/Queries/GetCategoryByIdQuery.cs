using Application.Services;

using AutoMapper;

using Common.Responses.Categories;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<ResponseWrapper<CategoryResponse>>
{
    public int CategoryId { get; set; }
}

public class GetCategoryByIdQueryHandler(ICategoryService categoryService, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, ResponseWrapper<CategoryResponse>>
{
    public async Task<ResponseWrapper<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryService.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return await ResponseWrapper<CategoryResponse>.FailAsync("Category not found.");
        }
        return await ResponseWrapper<CategoryResponse>.SuccessAsync(mapper.Map<CategoryResponse>(category));
    }
}