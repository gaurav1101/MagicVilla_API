using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService:BaseService, IVillaNumberService
    {
            private string url;
            private readonly IBaseService _service;

            public VillaNumberService(IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
            {
                
            //_service = service;
                url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            }

            public Task<T> CreateAsync<T>(VillaNumberCreateDto villaNumberCreateDto,string token)
            {
                return sendAsync<T>(new APIRequest
                {
                    url = url + "api/VillaNumberAPI",
                    apiType = SD.APIType.POST,
                    Data = villaNumberCreateDto,
                    Token= token
                });
            }

            public Task<T> GetAllAsync<T>(string token)
            {
                return sendAsync<T>(new APIRequest
                {
                    url = url + "api/VillaNumberAPI",
                    apiType = SD.APIType.GET,
                    Token= token
                });
            }

            public Task<T> GetAsync<T>(int id, string token)
            {
                return sendAsync<T>(new APIRequest
                {
                    url = url + "api/VillaNumberAPI/" + id,
                    apiType = SD.APIType.GET,
                     Token = token
                });
            }

            public Task<T> RemoveAsync<T>(int id, string token)
            {
                return sendAsync<T>(new APIRequest
                {
                    url = url + "api/VillaNumberAPI/" + id,
                    apiType = SD.APIType.DELETE,
                    Token= token
                });
            }

            public Task<T> UpdateAsync<T>(VillaNumberUpdateDto villaNumberUpdateDto, string token)
            {
                return sendAsync<T>(new APIRequest
                {
                    url = url + "api/VillaNumberAPI/" + villaNumberUpdateDto.VillaId,
                    apiType = SD.APIType.PUT,
                    Data = villaNumberUpdateDto,
                    Token= token
                });
            }
        }
    }



