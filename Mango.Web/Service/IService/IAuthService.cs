using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface IAuthService
{
    Task<ReponseDto?> LoginAsync(LoginRequestDto loginRequestDto);

    Task<ReponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

    Task<ReponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO);
}