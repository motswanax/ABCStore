using Application.Services;

using AutoMapper;

using Common.Responses.Products;
using Common.Wrappers;

using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductsByCategoryIdQuery : IRequest<ResponseWrapper<IEnumerable<ProductResponse>>>
{
    public int CategoryId { get; set; }
}

public class GetProductsByCategoryIdQueryHandler(IProductService productService, IMapper mapper) : IRequestHandler<GetProductsByCategoryIdQuery, ResponseWrapper<IEnumerable<ProductResponse>>>
{
    public async Task<ResponseWrapper<IEnumerable<ProductResponse>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var products = await productService.GetProductsByCategoryIdAsync(request.CategoryId, cancellationToken);

        if (products.Count() == 0)
        {
            return new ResponseWrapper<IEnumerable<ProductResponse>>().Fail("No products found for the specified category.");
        }

        var productResponses = mapper.Map<IEnumerable<ProductResponse>>(products);
        return new ResponseWrapper<IEnumerable<ProductResponse>>().Success(productResponses);
    }
}