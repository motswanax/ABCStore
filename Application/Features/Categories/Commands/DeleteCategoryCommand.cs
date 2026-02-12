using Application.Services;

using Common.Wrappers;

using MediatR;

namespace Application.Features.Categories.Commands;

public class DeleteCategoryCommand : IRequest<ResponseWrapper<int>>
{
    public int CategoryId { get; set; }
}

public class DeleteCategoryCommandHandler(ICategoryService categoryService) 
    : IRequestHandler<DeleteCategoryCommand, ResponseWrapper<int>> 
{ 
    public async Task<ResponseWrapper<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) 
    { 
        var categoryToDelete = await categoryService.GetByIdAsync(request.CategoryId, cancellationToken); 
        if (categoryToDelete == null) 
        { 
            return new ResponseWrapper<int>().Fail("Category not found."); 
        } 
        await categoryService.DeleteAsync(categoryToDelete, cancellationToken); 
        return new ResponseWrapper<int>().Success(request.CategoryId, "Category deleted successfully."); 
    } 
}