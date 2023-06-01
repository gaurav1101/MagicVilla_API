using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto);

        Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto);
    }
}
