using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories
{
    public interface ITransporteRepository
    {
        IUnityOfWork UnitOfWork { get; }

        Task AdicionarAsync(Transporte transporte, CancellationToken cancellationToken);

        Task<IEnumerable<Transporte>> BuscarPorCodigoMovimentacaoAsync(
            int codigoMovimentacao,
            CancellationToken cancellationToken);

        Task<IEnumerable<Transporte>> BuscarPorPlacaVeiculoAsync(
            string placaVeiculo,
            CancellationToken cancellationToken);

        Task AtualizarDataEntregaAsync(int codigoMovimentacao, CancellationToken cancellationToken);
    }
}
