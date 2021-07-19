using AutoMapper;
using RelatedProductsApi.Data.Entities;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RelatedProduct, RelatedProductEntity>().ReverseMap();
            CreateMap<GetByPageResponse, PagingDataResult>()
                .ForMember(dest => dest.RelatedProductsEntity, opt => opt.MapFrom(src => src.RelatedProducts))
                .ReverseMap();
        }
    }
}
