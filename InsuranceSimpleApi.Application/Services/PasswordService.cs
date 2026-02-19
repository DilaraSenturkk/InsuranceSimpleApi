using InsuranceSimpleApi.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace InsuranceSimpleApi.Application.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string Hash(User user, string password)
            => _hasher.HashPassword(user, password);

        public bool Verify(User user, string password)
            => _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            ) == PasswordVerificationResult.Success;
    }
}
