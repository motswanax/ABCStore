using Application.Services;

using Common.Wrappers;

using MediatR;

namespace Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<ResponseWrapper<int>>
{
    public required int ProductId { get; set; }
}

public class DeleteProductCommandHandler(IProductService productService) 
    : IRequestHandler<DeleteProductCommand, ResponseWrapper<int>> 
{ 
    public async Task<ResponseWrapper<int>> Handle(DeleteProductCommand request, CancellationToken cancellationToken) 
    { 
        var productToDelete = await productService.GetByIdAsync(request.ProductId, cancellationToken); 
        if (productToDelete == null) 
        { 
            return await ResponseWrapper<int>.FailAsync("Product not found."); 
        } 
        await productService.DeleteAsync(productToDelete, cancellationToken); 
        return await ResponseWrapper<int>.SuccessAsync(request.ProductId, "Product deleted successfully."); 
    } 
}