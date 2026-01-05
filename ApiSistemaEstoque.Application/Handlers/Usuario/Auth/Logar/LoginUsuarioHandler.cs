using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Logar;

public class LoginUsuarioHandler 
    : IRequestHandler<LoginUsuarioRequest, LoginUsuarioResponse>
{
    private readonly IUsuarioAuthService _authService;

    public LoginUsuarioHandler(IUsuarioAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginUsuarioResponse> Handle(
        LoginUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        var usuario = await _authService.AutenticarAsync(
            request.Email,
            request.Senha,
            cancellationToken);

        if (usuario is null)
            throw new ApplicationException("Usuário ou senha inválidos.");

        return new LoginUsuarioResponse(
            usuario.Id,
            usuario.Email,
            usuario.Token
        );
    }
}
