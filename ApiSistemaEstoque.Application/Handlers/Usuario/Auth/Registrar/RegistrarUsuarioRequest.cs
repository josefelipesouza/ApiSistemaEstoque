using MediatR;

namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;

public class RegistrarUsuarioRequest : IRequest<RegistrarUsuarioResponse>
{
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public string Role { get; set; } = default!;
    public int CodigoEstoque { get; set; } = default!;
}
