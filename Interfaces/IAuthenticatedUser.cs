namespace InsuranceSimpleApi.Interfaces
{
    public interface IAuthenticatedUser
    {
        int? UserId { get; }
        string? Name { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
