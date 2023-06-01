using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _service;
        private readonly IMapper _mapper;

        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _service = villaService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync<Response>(SD.AuthToken);
            List<VillaDto> villaDtos = new();
            if (response != null && response.IsSuccess)
            {
                villaDtos = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(villaDtos);
        }

    }
}