using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

    public interface IEstoqueRepository
    {
        IUnityOfWork UnitOfWork { get; }

        Task AdicionarAsync(Estoque estoque, CancellationToken cancellationToken);

        Task<Estoque?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken);

        Task<IEnumerable<Estoque>> ListarAsync(CancellationToken cancellationToken);

        void Atualizar(Estoque estoque);

        void Inativar(Estoque estoque);
    }

