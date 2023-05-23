using Quiz.Models.Entities;
using Quiz.Models.Enums;

namespace Quiz.Models.Dto
{
    public class LoginRequestUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Role { get; set; }    
        public string Token { get; set; }
    }

    public class RegisterRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
