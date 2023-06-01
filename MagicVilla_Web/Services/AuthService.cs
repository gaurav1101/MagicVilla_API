using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;

namespace MagicVilla_Web.Services
{
    public class AuthService:BaseService, IAuthService 
    {
        private readonly IConfiguration _config;
        private string _url;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public AuthService(IConfiguration configuration,IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        { 
            _config = configuration;
            _url = _config.GetValue<string>("ServiceUrls:VillaAPI");

        }
        public Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto)
        {
            return sendAsync<T>(new APIRequest
            {
                Data = loginRequestDto,
                url = _url + "api/UserAuth/Login",
                apiType = SD.APIType.POST
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto)
        {
            return sendAsync<T>(new APIRequest
            {
                Data = registerationRequestDto,
                url = _url + "api/UserAuth/Register",
                apiType = SD.APIType.POST
            });
        }
    }
}
