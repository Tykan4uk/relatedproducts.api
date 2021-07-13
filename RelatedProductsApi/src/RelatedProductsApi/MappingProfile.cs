using AutoMapper;
using RelatedProductsApi.Data;
using RelatedProductsApi.Models;

namespace RelatedProductsApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RelatedProduct, RelatedProductEntity>().ReverseMap();
        }
    }
}
