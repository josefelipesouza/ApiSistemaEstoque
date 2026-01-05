using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Auth;

public class UsuarioAuthService : IUsuarioAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UsuarioAuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
            throw new ApplicationException(
                string.Join(", ", resultado.Errors.Select(e => e.Description)));

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

        var resultado = await _signInManager.PasswordSignInAsync(
            usuario, senha, false, false);

        if (!resultado.Succeeded)
            return null;

        return new UsuarioAutenticadoDto
        {
            Id = usuario.Id,
            Email = usuario.Email!
            
        };
    }
}
