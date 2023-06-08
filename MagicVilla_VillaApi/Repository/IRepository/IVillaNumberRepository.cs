using MagicVilla_VillaApi.Models;

namespace MagicVilla_VillaApi.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateVillaNumberAsync(VillaNumber villaNumber);
    }
}
