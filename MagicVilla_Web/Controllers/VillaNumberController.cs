using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _service;
        private readonly IMapper _mapper;
      private readonly IVillaService _villa;
        public VillaNumberController(IVillaService villa,IVillaNumberService villaService, IMapper mapper)
        {
            _service = villaService;
            _mapper = mapper;
            _villa = villa;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            var response = await _service.GetAllAsync<Response>();
            List<VillaNumberDto> villaDtos = new();
            if (response != null && response.IsSuccess)
            {
                villaDtos = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
            }
            return View(villaDtos);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await _villa.GetAllAsync<Response>();
            VillaNumberCreateVM createDtoVm = new();
            if (response != null && response.IsSuccess)
            {
                createDtoVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); ;
            }
            return View(createDtoVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.CreateAsync<Response>(_mapper.Map<VillaNumberCreateDto>(vm.villaNumberDto));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
                }

                var data = await _villa.GetAllAsync<Response>();
                VillaNumberCreateVM createDtoVm = new();
                if (data != null && data.IsSuccess)
                {
                    createDtoVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                        (Convert.ToString(data.Result)).Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        }); ;
                    return View(createDtoVm);
                }
            }
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> UpdateVillaNumber(int Id)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.GetAsync<Response>(Id);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                return View(response);
            }
            return View();
        }

        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDto update)
        {
            var response = await _service.UpdateAsync<Response>(update);
            VillaNumberCreateVM createDtoVm = new();
        
            return View(createDtoVm);
        }
    }
}
