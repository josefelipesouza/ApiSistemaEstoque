using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

public interface ITipoMovimentacaoRepository
{
    IUnityOfWork UnitOfWork { get; }

    Task AdicionarAsync(TipoMovimentacao tipoMovimentacao, CancellationToken cancellationToken);

    Task<TipoMovimentacao?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken);

    Task<IEnumerable<TipoMovimentacao>> ListarAsync(CancellationToken cancellationToken);

    void Atualizar(TipoMovimentacao tipoMovimentacao);
}
