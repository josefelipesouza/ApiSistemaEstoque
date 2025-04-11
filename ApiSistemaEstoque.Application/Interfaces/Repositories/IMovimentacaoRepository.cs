using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories
{
    public interface IMovimentacaoRepository
    {
        IUnityOfWork UnitOfWork { get; }

        Task AdicionarAsync(Movimentacao movimentacao, CancellationToken cancellationToken);

        Task AdicionarItemAsync(ItemMovimentacao itemMovimentacao, CancellationToken cancellationToken);

        Task<IEnumerable<Movimentacao>> ListarAsync(CancellationToken cancellationToken);

        Task<Movimentacao?> BuscarMovimentacaoPorCodigoAsync(int codigo, CancellationToken cancellationToken);

        Task<IEnumerable<Movimentacao>> BuscarMovimentacaoAsync(
            int codigoEstoqueSolicitante,
            int codigoEstoqueSolicitado,
            int codigoTipoMovimentacao,
            DateTime dataInicial,
            DateTime dataFinal,
            CancellationToken cancellationToken
        );

        void Atualizar(Movimentacao movimentacao);
    }
}
