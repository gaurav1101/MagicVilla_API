using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaApi.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretkey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDBContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper,RoleManager<IdentityRole> roleManager)
        {
            _dbContext = db;
            _mapper = mapper;
            _userManager = userManager;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            return (_dbContext.ApplicationUsers.FirstOrDefault(u=>u.UserName == username)==null)?true :false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid= await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            
            if (user == null && !isValid)
            {
                LoginResponseDto loginResponse1 = new LoginResponseDto
                {
                    Token = "",  //to convert token to string
                    User = null
                };
                return loginResponse1;
            }

            //if user is foud then we will do JWT auth and return a token

            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
             var roles= await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName) ,  //need key and values per claim
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponse = new LoginResponseDto
            {
                Token = tokenhandler.WriteToken(token),  //to convert token to string
                User =new UserDto()
                {
                    applicationUser= user,
                }
            };
            return loginResponse;
        }

        public async Task<UserDto> Register(RegisterationRequestDto registerationRequestDto)
        {
                ApplicationUser _user =
                     _user = new ApplicationUser
                     {
                         UserName = registerationRequestDto.UserName,
                         Email = registerationRequestDto.UserName,
                         NormalizedEmail = registerationRequestDto.UserName.ToUpper(),
                         Name = registerationRequestDto.Name,
                         Discriminator= "hhhdj"
                     };
            try
            {
                var response= await _userManager.CreateAsync(_user,registerationRequestDto.Password);
                if (response.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(_user, "admin");
                    var userReturn= _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower()==_user.UserName.ToLower());
                    return _mapper.Map<UserDto>(userReturn); 
                }
            }
            catch (Exception ex)
            {
                
            }
            UserDto user = new();
            return user;
        }
    }
}
