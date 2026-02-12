using InsuranceSimpleApi.Interfaces;

namespace InsuranceSimpleApi.Models
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
