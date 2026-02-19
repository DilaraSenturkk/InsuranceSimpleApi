using InsuranceSimpleApi.Domain.Models;

namespace InsuranceSimpleApi.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}