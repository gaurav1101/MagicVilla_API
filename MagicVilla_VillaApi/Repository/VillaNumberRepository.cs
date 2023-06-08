using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repository.IRepository;

namespace MagicVilla_VillaApi.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>,IVillaNumberRepository
    {
        private readonly ApplicationDBContext _dBContext;
        public VillaNumberRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dBContext = applicationDBContext;
        }

        public async Task<VillaNumber> UpdateVillaNumberAsync(VillaNumber villaNumber)
        {
                _dBContext.VillaNumbers.Update(villaNumber);
                await _dBContext.SaveChangesAsync();
                return villaNumber;
        }
    }
}
