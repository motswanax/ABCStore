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

public class UpdateProductCommandHandler(IProductService productService, IMapper mapper) 
    : IRequestHandler<UpdateProductCommand, ResponseWrapper<int>> 
{ 
    public async Task<ResponseWrapper<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) 
    {
        var productToUpdate = await productService.GetByIdAsync(request.Request.Id, cancellationToken);
        if (productToUpdate == null) 
        { 
            return new ResponseWrapper<int>().Fail("Product not found.");
        }

        productToUpdate.Name = request.Request.Name;
        productToUpdate.Description = request.Request.Description;
        productToUpdate.Price = request.Request.Price;
        productToUpdate.CategoryId = request.Request.CategoryId;
        var updatedProductId = await productService.UpdateAsync(productToUpdate, cancellationToken);
        return new ResponseWrapper<int>().Success(updatedProductId, "Product updated successfully."); 
    }
}