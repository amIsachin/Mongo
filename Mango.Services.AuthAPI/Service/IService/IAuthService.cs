using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Service.IService;

public interface IAuthService
{
    public Task<string> Registration(RegistrationRequestDTO registrationRequestDTO);

    public Task<LoginResponseDto> Login(LoginRequestDto loginResponseDTO);

    public Task<bool> AssignRole(string email, string roleName);
}
