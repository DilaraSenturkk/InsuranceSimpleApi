using InsuranceSimpleApi.Application.DTOs;
using InsuranceSimpleApi.Application.Interfaces;
using InsuranceSimpleApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceSimpleApi.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var (accessToken, refreshToken) = await _authService.LoginAsync(dto);
            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefreshDto dto)
        {
            var (accessToken, refreshToken) = await _authService.RefreshTokenAsync(dto);
            return Ok(new { accessToken, refreshToken });
        }
    }
}