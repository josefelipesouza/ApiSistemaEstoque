using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

public interface IItemRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task AdicionarAsync(Item item, CancellationToken cancellationToken);

    Task<Item?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken);

    Task<IEnumerable<Item>> ListarAsync(CancellationToken cancellationToken);

    void Atualizar(Item item);

    void Inativar(Item item);
}
