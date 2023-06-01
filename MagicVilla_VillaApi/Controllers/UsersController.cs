using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaApi.Controllers
{
    [ApiController]
    [Route("api/UserAuth")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private Response _response;
        public UsersController(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
            _response = new Response();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult<Response> Register(RegisterationRequestDto registerationRequest)
        {
            bool valid = _userRepository.IsUniqueUser(registerationRequest.UserName);
            if (!valid)
            {
                _response.ErrorMessage = new List<string> { "User already Exists" };
                _response.IsSuccess = false;
                    return BadRequest(_response);
            }
            var user= _userRepository.Register(registerationRequest);
            if(user == null)
            {
                _response.ErrorMessage = new List<string> { "Invalid User" };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = registerationRequest;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("Login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var loginUser= _userRepository.Login(loginRequest);
            if(loginUser.Result.User==null && loginUser.Result.Token == "")
            {
                _response.ErrorMessage = new List<string> { "Username or Password is incorrect" };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = loginUser.Result;
                _response.IsSuccess = true;
                return Ok(_response);
        }
    }
}
