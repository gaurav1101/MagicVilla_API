using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repository.IRepository;

namespace MagicVilla_VillaApi.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>,IVillaNumberRepository
    {
        public VillaNumberRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            
        }

        public Task UpdateVillaNumberAsync(VillaNumber villaNumber)
        {
            throw new NotImplementedException();
        }
    }
}
