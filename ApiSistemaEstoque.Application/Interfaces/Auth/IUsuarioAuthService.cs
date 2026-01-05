using System.Threading;
using System.Threading.Tasks;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

public interface IUsuarioAuthService
{
    Task<UsuarioAutenticadoDto?> AutenticarAsync(
        string email,
        string senha,
        CancellationToken cancellationToken);

    Task<UsuarioRegistradoDto> RegistrarAsync(
        string email,
        string senha,
        string role,
        CancellationToken cancellationToken);
}
