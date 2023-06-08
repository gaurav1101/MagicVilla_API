using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Models;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Response = MagicVilla_Web.Models.Response;
using VillaNumberDto = MagicVilla_Web.Models.Dto.VillaNumberDto;
using VillaDto = MagicVilla_Web.Models.Dto.VillaDto;
using MagicVilla_Web.Services;
using VillaNumberCreateDto = MagicVilla_Web.Models.Dto.VillaNumberCreateDto;
using VillaNumberUpdateDto = MagicVilla_Web.Models.Dto.VillaNumberUpdateDto;

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

        //[Authorize(Roles = "admin")]
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

        //[Authorize(Roles = "admin")]
        //[HttpPost]
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
                        }); 
                    return View(createDtoVm);
                }
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateVillaNumber(int VillaNo)
        {
            if (ModelState.IsValid)
            {
                var response = await _numberService.GetAsync<Response>(VillaNo, HttpContext.Session.GetString(SD.AuthToken));
                VillaNumberUpdateVM villaNumberUpdateVM=new();
                if (response != null && response.IsSuccess)
                {

                    var dto = JsonConvert.DeserializeObject<VillaNumberUpdateDto>(Convert.ToString(response.Result));
                    villaNumberUpdateVM.updateDto
                    = _mapper.Map<VillaNumberUpdateDto>(dto);
                    //return View(villaNumberCreateVM);
                }
                response = await _villa.GetAllAsync<Response>(HttpContext.Session.GetString(HttpContext.Session.GetString(SD.AuthToken)));
                if (response != null && response.IsSuccess)
                {
                    villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                        (Convert.ToString(response.Result)).Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                    return View(villaNumberUpdateVM);
                }
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM update)
        {
            var response = await _numberService.UpdateAsync<Response>(update, HttpContext.Session.GetString(SD.AuthToken));
            var dto = JsonConvert.DeserializeObject<VillaNumberUpdateVM>(Convert.ToString(response.Result));

            return View(dto);
        }
    }
}
