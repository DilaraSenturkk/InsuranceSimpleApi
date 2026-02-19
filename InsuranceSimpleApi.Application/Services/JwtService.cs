using InsuranceSimpleApi.Application.Interfaces;
using InsuranceSimpleApi.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) => _config = config;

    public string GenerateToken(User user)
    {
        // 1. Kullanıcı bilgilerini (Claims) tanımlayın
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

        // 2. appsettings.json dosyasındaki Key ile imzalama anahtarı oluşturun
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        // 3. Algoritmayı belirleyerek kimlik bilgilerini oluşturun
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. Token özelliklerini belirleyin (Issuer, Audience, Süre vb.)
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        // 5. Token'ı string formatında döndürün
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}