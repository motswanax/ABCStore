using AutoMapper;

using Common.Requests.Categories;
using Common.Requests.Products;
using Common.Responses.Categories;
using Common.Responses.Products;

using Domain;

namespace Application;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();

        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
    }
}
