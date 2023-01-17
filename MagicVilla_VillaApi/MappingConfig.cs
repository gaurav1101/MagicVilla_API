using AutoMapper;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;

namespace MagicVilla_VillaApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<VillaCreateDto, VillaDto>().ReverseMap();
            CreateMap<VillaCreateDto, Villa>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();
        }
    }
}
