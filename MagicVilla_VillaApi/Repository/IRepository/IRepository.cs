using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        //int pagesize and int pagenumber are added to enable pagination 

        Task<List<T>> getAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,int pageSize=3,int pageNumber=1); 
        Task<T> getFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task RemoveAsync(T entity);
        void SaveAsync();
    }
}
