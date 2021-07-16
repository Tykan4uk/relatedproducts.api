using System;
using AutoMapper;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Requests;
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
            CreateMap<AddRequest, RelatedProductEntity>().BeforeMap((src, dest) => dest.Id = Guid.NewGuid().ToString());
        }
    }
}
