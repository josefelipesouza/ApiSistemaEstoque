using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

    public class EstoqueRepository : IEstoqueRepository
    {
        public IUnitOfWork UnitOfWork => _context;

        private readonly EstoqueContext _context;

        public EstoqueRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Estoque estoque, CancellationToken cancellationToken)
        {
            await _context.Estoques.AddAsync(estoque, cancellationToken);
        }

        public async Task<Estoque?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken)
        {
            return await _context.Estoques
                .AsNoTracking()
                .Where(x => x.Codigo == codigo)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<Estoque>> ListarAsync(CancellationToken cancellationToken)
        {
            {
                return await _context.Estoques
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }

        public void Atualizar(Estoque estoque)
        {
            _context.Estoques.Update(estoque);
        }

        public void Inativar(Estoque estoque)
        {
            estoque.SetInativar();

            _context.Estoques.Update(estoque);
        }


    }

