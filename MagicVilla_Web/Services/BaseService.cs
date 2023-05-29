using MagicVilla_Utility;
using MagicVilla_Web.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public Response responseModel { get ; set ; }
        public IHttpClientFactory httpClient { get;set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;

        }
        public async Task<T> sendAsync<T>(APIRequest apirequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage httpRequest = new HttpRequestMessage();
                httpRequest.Headers.Add("Accept","application/json");
                httpRequest.RequestUri = new Uri(apirequest.url);
                if (apirequest.Data != null)
                {
                    httpRequest.Content = new StringContent(JsonConvert.SerializeObject(apirequest.Data),Encoding.UTF8,"application/json");
                }
                switch (apirequest.apiType)
                {
                        case SD.APIType.POST:
                        httpRequest.Method = HttpMethod.Post;
                        break;
                        case SD.APIType.PUT:
                        httpRequest.Method = HttpMethod.Put;
                        break;
                        case SD.APIType.DELETE:
                        httpRequest.Method = HttpMethod.Delete;
                        break;
                        default:
                        httpRequest.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage httpResponse = null; 
                httpResponse = await client.SendAsync(httpRequest);
                var apiContent = await httpResponse.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;
            }
            catch (Exception ex)
            {
                var dto = new Response
                {
                    ErrorMessage = new List<string> { ex.Message },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }
        }
    }
}