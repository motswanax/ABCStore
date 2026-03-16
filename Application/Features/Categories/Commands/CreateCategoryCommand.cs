using Application.Services;

using AutoMapper;

using Common.Pipelines;
using Common.Requests.Categories;
using Common.Wrappers;

using Domain;

using MediatR;

namespace Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<ResponseWrapper<int>>, IValidateMe
{
    public required CreateCategoryRequest Request { get; set; }
}

public class CreateCategoryCommandHandler(ICategoryService categoryService, IMapper mapper) : IRequestHandler<CreateCategoryCommand, ResponseWrapper<int>>
{
    public async Task<ResponseWrapper<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = await categoryService.CreateAsync(mapper.Map<CreateCategoryRequest, Category>(request.Request), cancellationToken);
        return await ResponseWrapper<int>.SuccessAsync(newCategory.Id, "Category created successfully.");
    }
}