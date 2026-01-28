namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GerarToken(
        string userId,
        string email,
        IEnumerable<string> roles);
}
