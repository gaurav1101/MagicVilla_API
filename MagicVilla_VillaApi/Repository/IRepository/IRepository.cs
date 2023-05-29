using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task<List<T>> getAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> getFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task RemoveAsync(T entity);
        void SaveAsync();
    }
}
