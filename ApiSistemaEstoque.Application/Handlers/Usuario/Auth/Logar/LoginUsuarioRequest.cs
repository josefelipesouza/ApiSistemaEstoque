using ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Logar;
using MediatR;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Logar;

public class LoginUsuarioRequest : IRequest<LoginUsuarioResponse>
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
