namespace InsuranceSimpleApi.Application.DTOs.Admin
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; 
    }
}
