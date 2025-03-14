﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();

            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ReponseDto reponseDto = await _authService.LoginAsync(loginRequestDto);

            if (reponseDto != null && reponseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(reponseDto.Result));

                await SignInUser(loginResponseDto!);

                _tokenProvider.SetToken(loginResponseDto!.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Customer error", reponseDto!.Message);
                return View(loginRequestDto);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>
            {
                new SelectListItem{ Text=SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{ Text=SD.RoleCustomer, Value=SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ReponseDto result = await _authService.RegisterAsync(registrationRequestDTO);

            ReponseDto assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                {
                    registrationRequestDTO.Role = SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["Success"] = "Registration successful";
                    return RedirectToAction(nameof(Login));

                }
            }

            var roleList = new List<SelectListItem>
            {
                new SelectListItem{ Text=SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{ Text=SD.RoleCustomer, Value=SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View(registrationRequestDTO);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            _tokenProvider.ClearToken();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)!.Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));

            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role")!.Value));

            var claimPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
        }
    }
}
