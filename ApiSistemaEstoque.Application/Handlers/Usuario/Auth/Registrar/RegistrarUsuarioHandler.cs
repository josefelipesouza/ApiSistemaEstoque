
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;

public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioRequest, RegistrarUsuarioResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegistrarUsuarioHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<RegistrarUsuarioResponse> Handle(RegistrarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var usuario = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Senha);

        if (!resultado.Succeeded)
            throw new ApplicationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync(request.Role))
            throw new ApplicationException("Role não existe.");

        await _userManager.AddToRoleAsync(usuario, request.Role);

        return new RegistrarUsuarioResponse(usuario.Id, usuario.Email, request.Role);
    }
}
