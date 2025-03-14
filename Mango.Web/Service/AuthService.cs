using Mango.Services.CouponAPI.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using System;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<Mango.Web.Models.ReponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDTO,
            Url = AuthAPIBase + "/api/AuthAPI/AssignRole"
        });
    }

    public async Task<Mango.Web.Models.ReponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = loginRequestDto,
            Url = AuthAPIBase + "/api/AuthAPI/Login"
        }, withBearer: false);
    }

    public async Task<Mango.Web.Models.ReponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = registrationRequestDTO,
            Url = AuthAPIBase + "/api/AuthAPI/Register"
        }, withBearer: false);
    }
}