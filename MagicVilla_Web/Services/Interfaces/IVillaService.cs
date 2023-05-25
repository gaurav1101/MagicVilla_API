using MagicVilla_Web.Models.Dto;
using System.Linq.Expressions;

namespace MagicVilla_Web.Services.Interfaces
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDto villaCreateDto);
        Task<T> UpdateAsync<T>(VillaUpdateDto villaUpdateDto);
        Task<T> RemoveAsync<T>(int id);
        
    }
}
