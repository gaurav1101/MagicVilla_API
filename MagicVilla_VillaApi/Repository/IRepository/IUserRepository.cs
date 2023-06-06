using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;

namespace MagicVilla_VillaApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<UserDto> Register(RegisterationRequestDto registerationRequestDto);
    }
}
