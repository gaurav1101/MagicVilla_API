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

        public Task<T> CreateAsync<T>(VillaCreateDto villaCreateDto)
        {
           return sendAsync<T>(new APIRequest 
           { 
           url = url + "/api/VillaAPI",
           apiType= SD.APIType.POST,
           Data= villaCreateDto
           });
        }

        public Task<T> GetAllAsync<T>()
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "/api/VillaAPI",
                apiType = SD.APIType.GET,
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "/api/VillaAPI"+id,
                apiType = SD.APIType.GET,
            });
        }

        public Task<T> RemoveAsync<T>(int id)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "/api/VillaAPI" + id,
                apiType = SD.APIType.DELETE,
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDto villaUpdateDto)
        {
            return sendAsync<T>(new APIRequest
            {
                url = url + "/api/VillaAPI",
                apiType = SD.APIType.PUT,
                Data = villaUpdateDto
            });
        }
    }
}
