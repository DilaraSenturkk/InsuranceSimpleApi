using InsuranceSimpleApi.Application.DTOs;

namespace InsuranceSimpleApi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginDto dto);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(TokenRefreshDto dto);
    }
}