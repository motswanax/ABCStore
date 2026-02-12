using Application.Services;

using AutoMapper;

using Common.Responses.Categories;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Categories.Queries;

public class GetCategoriesQuery : IRequest<ResponseWrapper<IEnumerable<CategoryResponse>>>
{
}

public class GetCategoriesQueryHandler(ICategoryService categoryService, IMapper mapper) 
    : IRequestHandler<GetCategoriesQuery, ResponseWrapper<IEnumerable<CategoryResponse>>>
{
    public async Task<ResponseWrapper<IEnumerable<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);
        return new ResponseWrapper<IEnumerable<CategoryResponse>>().Success(mapper.Map<IEnumerable<CategoryResponse>>(categories));
    }
}