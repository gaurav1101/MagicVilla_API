using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _numberService;
        private readonly IMapper _mapper;
        private readonly IVillaService _villa;
        private string token;
        public VillaNumberController(IVillaService villa,IVillaNumberService villaService, IMapper mapper)
        {
            _numberService = villaService;
            _mapper = mapper;
            _villa = villa;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            var response = await _numberService.GetAllAsync<Response>(HttpContext.Session.GetString(SD.AuthToken));
            List<VillaNumberDto> villaDtos = new();
            if (response != null && response.IsSuccess)
            {
                villaDtos = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
            }
            return View(villaDtos);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await _villa.GetAllAsync<Response>(HttpContext.Session.GetString(SD.AuthToken));
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var response = await _numberService.CreateAsync<Response>(_mapper.Map<VillaNumberCreateDto>(vm.villaNumberDto), HttpContext.Session.GetString(SD.AuthToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
                }

                var data = await _villa.GetAllAsync<Response>(HttpContext.Session.GetString(SD.AuthToken));
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


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateVillaNumber(int Id)
        {
            if (ModelState.IsValid)
            {
                var response = await _numberService.GetAsync<Response>(Id, HttpContext.Session.GetString(SD.AuthToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                return View(response);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDto update)
        {
            var response = await _numberService.UpdateAsync<Response>(update, HttpContext.Session.GetString(SD.AuthToken));
            VillaNumberCreateVM createDtoVm = new();
        
            return View(createDtoVm);
        }
    }
}
