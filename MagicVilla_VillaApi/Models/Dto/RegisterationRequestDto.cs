namespace MagicVilla_VillaApi.Models.Dto
{
    public class RegisterationRequestDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Discriminator { get; set; }

    }
}
