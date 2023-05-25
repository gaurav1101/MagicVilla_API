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

            public async Task<List<T>> getAllAsync(Expression<Func<T, bool>> filter = null)
            {
                IQueryable<T> query = _dbSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.ToListAsync();
            }

            public async Task<T> getFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked)
            {
                IQueryable<T> query = _dbSet;
                if (!tracked)
                {
                    query = query.AsNoTracking();
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
