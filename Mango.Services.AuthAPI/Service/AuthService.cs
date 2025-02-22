using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJWTTokenGenerator jwtTokenGenerator)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());

        if (user is not null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                // Create the role here.
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        return false;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO)
    {
        var user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        if (user is null || !isValid)
        {
            return new LoginResponseDto() { User = null!, Token = string.Empty };
        }

        // If user was found, Genereate JWT Token.
        var token = _jwtTokenGenerator.GenerateToken(user);

        UserDTO userDTO = new UserDTO()
        {
            Email = user.Email!,
            ID = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber!
        };

        LoginResponseDto loginResponseDto = new LoginResponseDto()
        {
            User = userDTO,
            Token = token,
        };

        return loginResponseDto;

    }

    public async Task<string> Registration(RegistrationRequestDTO registrationRequestDTO)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = registrationRequestDTO.Email,
            Email = registrationRequestDTO.Email,
            NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
            Name = registrationRequestDTO.Name,
            PhoneNumber = registrationRequestDTO.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);

            if (result.Succeeded)
            {
                var userToReturn = _appDbContext.ApplicationUsers.First(x => x.UserName == registrationRequestDTO.Email);

                UserDTO userDto = new()
                {
                    Email = userToReturn.Email,
                    ID = userToReturn.Id,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                // return userDto;

                return string.Empty;
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
