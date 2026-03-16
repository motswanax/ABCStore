using Application.Services;

using AutoMapper;

using Common.Requests.Products;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<ResponseWrapper<int>>
{
    public required UpdateProductRequest Request { get; set; }
}

public class UpdateProductCommandHandler(IProductService productService) 
    : IRequestHandler<UpdateProductCommand, ResponseWrapper<int>> 
{ 
    public async Task<ResponseWrapper<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) 
    {
        var productToUpdate = await productService.GetByIdAsync(request.Request.Id, cancellationToken);
        if (productToUpdate == null) 
        { 
            return await ResponseWrapper<int>.FailAsync("Product not found.");
        }

        productToUpdate.Name = request.Request.Name;
        productToUpdate.Description = request.Request.Description;
        productToUpdate.Price = request.Request.Price;
        productToUpdate.CategoryId = request.Request.CategoryId;
        var updatedProductId = await productService.UpdateAsync(productToUpdate, cancellationToken);
        return await ResponseWrapper<int>.SuccessAsync(updatedProductId, "Product updated successfully."); 
    }
}