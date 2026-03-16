using Application.Services;

using AutoMapper;

using Common.Responses.Products;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductsQuery : IRequest<ResponseWrapper<IEnumerable<ProductResponse>>>
{
}

public class GetProductsQueryHandler(IProductService productService, IMapper mapper) 
    : IRequestHandler<GetProductsQuery, ResponseWrapper<IEnumerable<ProductResponse>>> 
{ 
    public async Task<ResponseWrapper<IEnumerable<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken) 
    { 
        var products = await productService.GetAllAsync(cancellationToken); 
        return await ResponseWrapper<IEnumerable<ProductResponse>>.SuccessAsync(mapper.Map<IEnumerable<ProductResponse>>(products)); 
    } 
}