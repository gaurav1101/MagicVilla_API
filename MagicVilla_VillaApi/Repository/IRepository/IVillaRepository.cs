using MagicVilla_VillaApi.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task AddVilla(Villa villa);
        Task<IEnumerable<Villa>> getAll();
        Task<Villa> getFirstOrDefault(Expression<Func<Villa, bool>> filter);
        Task<Villa> getById(int id);
        void delete(int id);
    }
}
