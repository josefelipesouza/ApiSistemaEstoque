namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

public class UsuarioRegistradoDto
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
}
