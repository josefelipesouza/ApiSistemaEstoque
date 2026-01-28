using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.Application.Interfaces.UsuariosEstoque;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;

public class RegistrarUsuarioHandler
    : IRequestHandler<RegistrarUsuarioRequest, RegistrarUsuarioResponse>
{
    private readonly IUsuarioAuthService _usuarioAuthService;
    private readonly IUsuarioEstoqueService _usuarioEstoqueService;

    public RegistrarUsuarioHandler(
        IUsuarioAuthService usuarioAuthService,
        IUsuarioEstoqueService usuarioEstoqueService)
    {
        _usuarioAuthService = usuarioAuthService;
        _usuarioEstoqueService = usuarioEstoqueService;
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

        await _usuarioEstoqueService.VincularAsync(
            usuario.Id,
            request.CodigoEstoque,
            cancellationToken
        );

        return new RegistrarUsuarioResponse(
            usuario.Id,
            usuario.Email,
            usuario.Role
        );
    }
}
