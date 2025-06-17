using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Logar;

public class LoginUsuarioHandler : IRequestHandler<LoginUsuarioRequest, LoginUsuarioResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public LoginUsuarioHandler(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<LoginUsuarioResponse> Handle(LoginUsuarioRequest request, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email);
        if (usuario == null)
            throw new ApplicationException("Usuário ou senha inválidos.");

        var resultado = await _signInManager.PasswordSignInAsync(usuario, request.Senha, false, false);
        if (!resultado.Succeeded)
            throw new ApplicationException("Usuário ou senha inválidos.");

        // Gerar token JWT com ID do usuário
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginUsuarioResponse(usuario.Id, usuario.Email, tokenString);
    }
}

