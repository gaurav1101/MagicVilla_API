using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.Interfaces
{
    public interface IVillaNumberService
    {
            Task<T> GetAllAsync<T>(string token);
            Task<T> GetAsync<T>(int id, string token);
            Task<T> CreateAsync<T>(VillaNumberCreateDto villaCreateDto, string token);
            Task<T> UpdateAsync<T>(VillaNumberUpdateDto villaUpdateDto, string token);
            Task<T> RemoveAsync<T>(int id, string token);
    }
}
