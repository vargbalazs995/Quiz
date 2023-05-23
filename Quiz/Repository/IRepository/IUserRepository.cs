using Quiz.Models.Dto;
using Quiz.Models.Entities;

namespace Quiz.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser (string username);
        Task<LoginResponseDTO> Login (LoginRequestUserDTO loginRequestUserDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
