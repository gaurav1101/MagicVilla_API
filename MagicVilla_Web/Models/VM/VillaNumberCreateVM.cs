using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberCreateVM
    {
        public VillaNumberCreateDto villaNumberDto { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
