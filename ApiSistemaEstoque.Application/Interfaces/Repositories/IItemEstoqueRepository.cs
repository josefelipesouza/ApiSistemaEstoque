using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories
{
    public interface IItemEstoqueRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task AdicionarAsync(ItemEstoque itemEstoque, CancellationToken cancellationToken);

        Task<IEnumerable<ItemEstoque>> ListarAsync(CancellationToken cancellationToken);

        Task AtualizarQuantidadeAsync(int codigoEstoque, int codigoItem, int quantidade, CancellationToken cancellationToken);

        Task<ItemEstoque?> BuscarPorCodigoEstoqueItem(int codigoEstoque, int codigoItem, CancellationToken cancellationToken);

        Task<IEnumerable<ItemEstoque>> BuscarPorCodigoEstoque(int codigoEstoque, CancellationToken cancellationToken);

    }
}
