using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;

public class RegistrarUsuarioHandler
    : IRequestHandler<RegistrarUsuarioRequest, RegistrarUsuarioResponse>
{
    private readonly IUsuarioAuthService _usuarioAuthService;

    public RegistrarUsuarioHandler(IUsuarioAuthService usuarioAuthService)
    {
        _usuarioAuthService = usuarioAuthService;
    }

    public async Task<RegistrarUsuarioResponse> Handle(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarioAuthService.RegistrarAsync(
            request.Email,
            request.Senha,
            request.Role,
            cancellationToken
        );

        return new RegistrarUsuarioResponse(
            usuario.Id,
            usuario.Email,
            usuario.Role
        );
    }
}
