using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public const string token = "AuthToken";

        public AuthController(IAuthService authService,IMapper mapper) 
        { 
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var user= await _authService.LoginAsync<Response>(loginRequestDto);
           
            if (user.Result!=null && user.IsSuccess)
            {
                var res = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(user.Result));

                //To enable auth after program.cs use below code
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, res.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, res.User.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                ///////

                HttpContext.Session.SetString(SD.AuthToken, res.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", user.ErrorMessage.FirstOrDefault());
                return View(loginRequestDto);
            }
        }

        [HttpGet]
       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequestDto)
        {
            var user = await _authService.RegisterAsync<Response>(registerationRequestDto);
            if (user.Result != null && user.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.AuthToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
