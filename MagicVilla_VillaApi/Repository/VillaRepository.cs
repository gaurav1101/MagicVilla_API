using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Migrations;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Repository
{
    public class VillaRepository : Repository<Villa> ,IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaRepository(ApplicationDBContext db) :base(db) 
        { 
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa villa)
        {
                _db.Villas.Update(villa);
                await _db.SaveChangesAsync();
                return villa;
        }
    }
}
