using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Migrations;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly DbSet<Villa> _dbSet;

        public VillaRepository(ApplicationDBContext db) 
        { 
            _db = db;
            _dbSet = db.Set<Villa>();
        }

        public async Task AddVilla(Villa villa)
        {
            await _db.Villas.AddAsync(villa);
        }
        public async Task<Villa> getById(int id)
        {
           var villa= await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa != null)
            {
                return villa;
            }
            return villa;
        }
        public async Task<IEnumerable<Villa>> getAll()
        {
           return await _db.Villas.ToListAsync();
        }
        public async Task<Villa> getFirstOrDefault(Expression<Func<Villa,bool>> filter)
        {
            Villa filterData=null;
            if (filter != null)
            {
                filterData = await _dbSet.FirstOrDefaultAsync(filter);
            }
            return filterData; 
        }
        public void delete(int id)
        {
         //var filterData = await _dbSet.FirstOrDefaultAsync();
         //   _dbSet.Remove(id);
        }
    }
}
