using AutoMapper;
using RelatedProductsApi.Data;
using RelatedProductsApi.Data.Entities;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RelatedProductModel, RelatedProductEntity>().ReverseMap();
            CreateMap<GetByPageResponse, PagingDataResult>()
                .ForMember(dest => dest.RelatedProductsEntity, opt => opt.MapFrom(src => src.RelatedProducts))
                .ReverseMap();
        }
    }
}
