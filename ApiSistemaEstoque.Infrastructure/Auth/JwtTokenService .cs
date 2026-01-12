using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(
        string userId,
        string email,
        IEnumerable<string> roles)
    {
        var jwtSection = _configuration.GetSection("Jwt");

        var secret = jwtSection["Secret"]!;
        var issuer = jwtSection["Issuer"]!;
        var audience = jwtSection["Audience"]!;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
