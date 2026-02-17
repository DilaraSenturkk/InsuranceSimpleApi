using InsuranceSimpleApi.Data;
using InsuranceSimpleApi.DTOs;
using InsuranceSimpleApi.Models;
using InsuranceSimpleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceSimpleApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context,PasswordService passwordService,JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Role = "User"
            };

            user.PasswordHash = _passwordService.Hash(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Kullanıcı oluşturuldu");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == dto.Username);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı");

            if (!_passwordService.Verify(user, dto.Password))
                return Unauthorized("Şifre hatalı");

            var accessToken = _jwtService.GenerateToken(user);

           
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges();

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(TokenRefreshDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.RefreshToken == dto.RefreshToken);

            if (user == null)
                return Unauthorized("Geçersiz refresh token");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token süresi dolmuş");

            var newAccessToken = _jwtService.GenerateToken(user);

 
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges();

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = user.RefreshToken
            });
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
