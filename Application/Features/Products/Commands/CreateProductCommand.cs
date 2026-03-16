using Application.Services;

using AutoMapper;

using Common.Pipelines;
using Common.Requests.Products;
using Common.Wrappers;

using Domain;

using MediatR;

namespace Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<ResponseWrapper<int>>
{
    public required CreateProductRequest Request { get; set; }
}

public class CreateProductCommandHandler(IProductService productService, IMapper mapper) 
    : IRequestHandler<CreateProductCommand, ResponseWrapper<int>> 
{ 
    public async Task<ResponseWrapper<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken) 
    { 
        var newProduct = await productService.CreateAsync(mapper.Map<Product>(request.Request), cancellationToken);
        return await ResponseWrapper<int>.SuccessAsync(newProduct.Id, "Product created successfully."); 
    } 
}