namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

public class UsuarioAutenticadoDto
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
    public string Token { get; init; } = default!;
}
