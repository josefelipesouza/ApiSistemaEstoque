using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Auth;

public class UsuarioAuthService : IUsuarioAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public UsuarioAuthService(
        UserManager<IdentityUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<UsuarioRegistradoDto> RegistrarAsync(
        string email,
        string senha,
        string role,
        CancellationToken cancellationToken)
    {
        var usuario = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var resultado = await _userManager.CreateAsync(usuario, senha);

        if (!resultado.Succeeded)
        {
            throw new ApplicationException(
                string.Join(", ", resultado.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(usuario, role);

        return new UsuarioRegistradoDto
        {
            Id = usuario.Id,
            Email = usuario.Email!,
            Role = role
        };
    }

    public async Task<UsuarioAutenticadoDto?> AutenticarAsync(
        string email,
        string senha,
        CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByEmailAsync(email);
        if (usuario is null)
            return null;

        var senhaValida = await _userManager.CheckPasswordAsync(usuario, senha);
        if (!senhaValida)
            return null;

        // 🔹 BUSCA ROLES DO USUÁRIO
        var roles = await _userManager.GetRolesAsync(usuario);

        // 🔹 GERA JWT (ASSINATURA CORRETA)
        var token = _jwtTokenService.GerarToken(
            usuario.Id,
            usuario.Email!,
            roles);

        return new UsuarioAutenticadoDto
        {
            Id = usuario.Id,
            Email = usuario.Email!,
            Token = token
        };
    }
}
