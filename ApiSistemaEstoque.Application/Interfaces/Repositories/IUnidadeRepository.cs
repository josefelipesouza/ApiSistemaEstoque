using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

public interface IUnidadeRepository
{
    IUnityOfWork UnitOfWork { get; }

    Task AdicionarAsync(Unidade unidade, CancellationToken cancellationToken);

    Task<Unidade?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken);

    Task<IEnumerable<Unidade>> ListarAsync(CancellationToken cancellationToken);

    void Atualizar(Unidade unidade);

    void Inativar(Unidade unidade);
}
