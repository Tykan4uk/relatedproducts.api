using AutoMapper;
using RelatedProductsApi.Models;

namespace RelatedProductsApi.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RelatedProduct, RelatedProductEntity>().ReverseMap();
        }
    }
}
