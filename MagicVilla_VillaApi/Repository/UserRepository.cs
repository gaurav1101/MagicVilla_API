using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
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
        private string secretkey;
        public UserRepository(ApplicationDBContext dBContext,IConfiguration config)
        {
            _dbContext = dBContext;
            secretkey = config.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            return (_dbContext.LocalUsers.FirstOrDefault(u=>u.UserName == username)==null)?true :false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower()
                                                            && u.Password == loginRequestDto.Password);
            if (user == null)
            {
                return null;
            }

            //if user is foud then we will do JWT auth and return a token

            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),  //need key and values per claim
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponse = new LoginResponseDto
            {
                Token = tokenhandler.WriteToken(token),  //to convert token to string
                User= user
            };
            return loginResponse;
        }

        public async Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto)
        {
            LocalUser _user=null;
            if (IsUniqueUser(registerationRequestDto.UserName))
            {
                 _user = new LocalUser
                {
                    UserName = registerationRequestDto.UserName,
                    Name=registerationRequestDto.Name,
                    Password=registerationRequestDto.Password,
                    Role = registerationRequestDto.Role
                };
                 await _dbContext.LocalUsers.AddAsync(_user);
                 await _dbContext.SaveChangesAsync();
               
            }
            else
            {
                
            }
            return _user;
        }
    }
}
