using System.Linq.Expressions;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories; 

public interface IUsuarioRepository
{
    IUnityOfWork UnityOfWork { get; }
    

    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken);
    
    void Atualizar(Usuario usuario);

    Task<bool> ExisteUsuario(
        Expression<Func<Usuario, bool>> filtroUsuario,
        CancellationToken cancellationToken
    );

    Task<Usuario?> BuscarPorIdAsync(int id, CancellationToken cancellationToken);

    Task<Usuario?> BuscarPorIdentityIdAsync(string identityId, CancellationToken cancellationToken);

    Task<( int? Id,
            string? CodigoCentroDeCusto,
            int? IdUsuarioCoordenador,
            string? Nome,
            string? Email,
            string? IdentityId,
            string? UsuarioWkId)>
        BuscarDadosCompletosPorIdAsync(
            int id,
            CancellationToken cancellationToken
        );
    
    Task<( int? Id,
            string? CodigoCentroDeCusto,
            int? IdUsuarioCoordenador,
            string? Nome,
            string? Email,
            string? IdentityId,
            string? UsuarioWkId)>
        BuscarDadosCompletosPorIdAsync(
            string identityId,
            CancellationToken cancellationToken
        );

    Task<IEnumerable<(int Id, string Nome, string Email, string Permissao)>?> BuscarAsync(
        string nome,
        string email,
        string? permissao,
        CancellationToken cancellationToken
    );
}