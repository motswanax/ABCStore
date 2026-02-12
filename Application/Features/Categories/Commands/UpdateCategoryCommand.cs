using Application.Services;

using Common.Requests.Categories;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<ResponseWrapper<int>>
{
    public required UpdateCategoryRequest Request { get; set; }
}

public class UpdateCategoryCommandHandler(ICategoryService categoryService) 
    : IRequestHandler<UpdateCategoryCommand, ResponseWrapper<int>>
{
    public async Task<ResponseWrapper<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await categoryService.GetByIdAsync(request.Request.Id);
        if (categoryToUpdate == null)
        {
            return new ResponseWrapper<int>().Fail("Category not found.");
        }
        categoryToUpdate.Name = request.Request.Name;
        categoryToUpdate.Description = request.Request.Description;
        var updatedCategoryId = await categoryService
            .UpdateAsync(categoryToUpdate);
        return new ResponseWrapper<int>().Success(updatedCategoryId, "Category updated successfully.");
    }
}