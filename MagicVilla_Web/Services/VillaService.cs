using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;

namespace MagicVilla_Web.Services
{
    public class VillaService :BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string url;
        
        public VillaService(IMapper mapper,IHttpClientFactory httpClientFactory,IConfiguration configuration): base(httpClientFactory) 
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaCreateDto villaCreateDto, string token)
        {
           return sendAsync<T>(new APIRequest 
           { 
           url = url + "api/VillaAPI",
           apiType= SD.APIType.POST,
           Data= villaCreateDto,
            Token=token
           });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "api/VillaAPI",
                apiType = SD.APIType.GET,
                Token= token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "api/VillaAPI/"+id,
                apiType = SD.APIType.GET,
                Token= token
            });
        }

        public Task<T> RemoveAsync<T>(int id, string token)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "api/VillaAPI/" + id,
                apiType = SD.APIType.DELETE,
                Token= token
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDto villaUpdateDto, string token)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "api/VillaAPI/"+ villaUpdateDto.Id,
                apiType = SD.APIType.PUT,
                Data = villaUpdateDto,
                Token= token
            });
        }
    }
}
