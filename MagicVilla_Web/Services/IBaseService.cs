using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services
{
    public interface IBaseService
    {
       Response responseModel { get; set; }
       Task<T> sendAsync<T>(APIRequest apirequest);
    }
}
