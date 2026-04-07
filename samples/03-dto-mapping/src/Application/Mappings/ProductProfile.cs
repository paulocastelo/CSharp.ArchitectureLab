using AutoMapper;
using DtoMapping.Sample.Application.Contracts;
using DtoMapping.Sample.Domain;

namespace DtoMapping.Sample.Application.Mappings;

public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<CreateProductRequest, Product>();
    }
}
