using MagicVilla_Web.Models.Dto;
using System.Linq.Expressions;

namespace MagicVilla_Web.Services.Interfaces
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaCreateDto villaCreateDto, string token);
        Task<T> UpdateAsync<T>(VillaUpdateDto villaUpdateDto, string token);
        Task<T> RemoveAsync<T>(int id, string token);
        
    }
}
