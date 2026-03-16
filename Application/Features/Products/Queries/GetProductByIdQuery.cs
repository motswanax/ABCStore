using Application.Services;

using AutoMapper;

using Common.Responses.Products;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<ResponseWrapper<ProductResponse>>
{
    public int ProductId { get; set; }
}

public class GetProductByIdQueryHandler(IProductService productService, IMapper mapper) 
    : IRequestHandler<GetProductByIdQuery, ResponseWrapper<ProductResponse>> 
{ 
    public async Task<ResponseWrapper<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) 
    { 
        var product = await productService.GetByIdAsync(request.ProductId, cancellationToken); 
        if (product == null) 
        { 
            return await ResponseWrapper<ProductResponse>.FailAsync("Product not found."); 
        } 
        return await ResponseWrapper<ProductResponse>.SuccessAsync(mapper.Map<ProductResponse>(product)); 
    } 
}