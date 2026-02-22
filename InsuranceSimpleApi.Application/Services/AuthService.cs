using InsuranceSimpleApi.Domain.Models; 
using InsuranceSimpleApi.Application.DTOs; 
using InsuranceSimpleApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using InsuranceSimpleApi.Infrastructure.Data;

namespace InsuranceSimpleApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(IApplicationDbContext context, PasswordService passwordService, IJwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            
            var user = new User { Username = dto.Username, Role = "User" };
            user.PasswordHash = _passwordService.Hash(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "Kullanıcı oluşturuldu";
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null || !_passwordService.Verify(user, dto.Password))
                throw new UnauthorizedAccessException("Geçersiz kullanıcı adı veya şifre");

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(TokenRefreshDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Geçersiz veya süresi dolmuş refresh token");

            var newAccessToken = _jwtService.GenerateToken(user);
            user.RefreshToken = GenerateRefreshToken();
            await _context.SaveChangesAsync();

            return (newAccessToken, user.RefreshToken);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}