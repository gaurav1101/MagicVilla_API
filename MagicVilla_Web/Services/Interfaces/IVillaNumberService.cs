using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.Interfaces
{
    public interface IVillaNumberService
    {
            Task<T> GetAllAsync<T>();
            Task<T> GetAsync<T>(int id);
            Task<T> CreateAsync<T>(VillaNumberCreateDto villaCreateDto);
            Task<T> UpdateAsync<T>(VillaNumberUpdateDto villaUpdateDto);
            Task<T> RemoveAsync<T>(int id);
    }
}
