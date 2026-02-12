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
            return new ResponseWrapper<CategoryResponse>().Fail("Category not found.");
        }
        return new ResponseWrapper<CategoryResponse>().Success(mapper.Map<CategoryResponse>(category));
    }
}