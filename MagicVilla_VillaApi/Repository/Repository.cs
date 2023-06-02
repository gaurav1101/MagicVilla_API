using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
            private readonly ApplicationDBContext _db;
            private readonly DbSet<T> _dbSet;

            public Repository(ApplicationDBContext db)
            {
                _db = db;
                _dbSet = db.Set<T>();
            }

            public async Task CreateAsync(T entity)
            {
                await _dbSet.AddAsync(entity);
                SaveAsync();
            }
        //u=>u.id==id
            public async Task<List<T>> getAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null, int pageSize = 3, int pageNumber = 1)
            {
                IQueryable<T> query = _dbSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                //skip0.take(5)
                //page number- 2     || page size -5
                //skip(5*(1)) take(5)
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(prop);
                }

            }
                return await query.ToListAsync();
            }

            public async Task<T> getFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked, string? includeProperties = null)
            {
                IQueryable<T> query = _dbSet;
                if (!tracked)
                {
                    query = query.AsNoTracking();
                }
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }

            }
            if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.FirstOrDefaultAsync();

            }
            public async Task RemoveAsync(T entity)
            {
                //var filterData = await _dbSet.FirstOrDefaultAsync();
                _dbSet.Remove(entity);
                SaveAsync();
            }

            public async void SaveAsync()
            {
            _db.SaveChangesAsync();
            }

    }
}
