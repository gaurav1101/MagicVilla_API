namespace MagicVilla_Web.Models.Dto
{
    public class LoginResponseDto
    {
        public LocalUserDto User { get; set; }
        public string Token { get; set; }
    }
}
