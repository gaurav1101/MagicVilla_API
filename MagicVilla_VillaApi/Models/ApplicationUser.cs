using Microsoft.AspNetCore.Identity;

namespace MagicVilla_VillaApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Discriminator { get; set; }
    }
}
