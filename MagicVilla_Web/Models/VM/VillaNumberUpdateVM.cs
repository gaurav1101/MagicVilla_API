using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaNumberUpdateDto = MagicVilla_Web.Models.Dto.VillaNumberUpdateDto;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberUpdateVM
    {
        public VillaNumberUpdateVM()
        {
            updateDto = new VillaNumberUpdateDto();
        }
        public VillaNumberUpdateDto updateDto { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }


    }
}
