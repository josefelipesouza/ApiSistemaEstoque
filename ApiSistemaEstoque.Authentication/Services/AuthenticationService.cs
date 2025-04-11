using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Services;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using Microsoft.AspNetCore.Identity;

//namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Services;
/*
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
public async Task<AuthenticationResponse> AuthenticateAsync
    public async Task AlterarPermissaoUsuario(Usuario usuario, IEnumerable<string> roles,
        CancellationToken cancellationToken)
    {
        var identityUser = await _userManager.FindByIdAsync(usuario);

        if (identityUser is null)
            return;

        var rolesUsuario = await _userManager.GetRolesAsync(identityUser);

        await _userManager.RemoveFromRolesAsync(identityUser, rolesUsuario);

        await _userManager.AddToRolesAsync(identityUser, roles);
    }
    

    
    public async Task<IEnumerable<string>> BuscarPermissoesUsuario(Usuario usuario, CancellationToken cancellationToken)
    {
        var identityUser = await _userManager.FindByIdAsync(usuario.codigo);

        if (identityUser is null)
            return [];

        return await _userManager.GetRolesAsync(identityUser);
    }
    
}
*/