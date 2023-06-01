using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _service;
        private readonly IMapper _mapper;
        private string token;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _service = villaService;
            _mapper = mapper;
          
        }

        public async Task<IActionResult> IndexVilla()
        {
            var response=await _service.GetAllAsync<Response>(HttpContext.Session.GetString(SD.AuthToken));
            List<VillaDto> villaDtos = new();
            if (response != null && response.IsSuccess)
            {
                villaDtos = JsonConvert.DeserializeObject <List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(villaDtos);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDto dto)
        {
            if (ModelState.IsValid)
            {
               var response= await _service.CreateAsync<Response>(dto, HttpContext.Session.GetString(SD.AuthToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
                return View(response);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response=await _service.GetAsync<Response>(villaId, HttpContext.Session.GetString(SD.AuthToken));
            VillaDto dto = new VillaDto();
            if(response!=null && response.IsSuccess)
            {
                dto=JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDto>(dto));
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.UpdateAsync<Response>(dto, HttpContext.Session.GetString(SD.AuthToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.RemoveAsync<Response>(villaId, HttpContext.Session.GetString(SD.AuthToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return NotFound();
        }
    }
}
