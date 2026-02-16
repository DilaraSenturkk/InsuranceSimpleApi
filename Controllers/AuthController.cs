using InsuranceSimpleApi.Data;
using InsuranceSimpleApi.DTOs;
using InsuranceSimpleApi.Models;
using InsuranceSimpleApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InsuranceSimpleApi.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public AuthController(
            AppDbContext context,
            PasswordService passwordService,
            JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User { Username = dto.Username };
            user.PasswordHash = _passwordService.Hash(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");

            if (!_passwordService.Verify(user, dto.Password))
                throw new UnauthorizedAccessException("Şifre hatalı");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }

}