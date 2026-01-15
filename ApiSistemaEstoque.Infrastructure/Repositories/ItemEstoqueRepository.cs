using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories
{
    public class ItemEstoqueRepository : IItemEstoqueRepository
    {
        public IUnityOfWork UnitOfWork => _context;

        private readonly EstoqueContext _context;

        public ItemEstoqueRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ItemEstoque itemEstoque, CancellationToken cancellationToken)
        {
            await _context.ItensEstoque.AddAsync(itemEstoque, cancellationToken);
        }

        public async Task<ItemEstoque?> BuscarPorCodigoEstoqueItem(int codigoEstoque, int codigoItem, CancellationToken cancellationToken)
        {
            return await _context.ItensEstoque
                .Include(ie => ie.Item)
                .Include(ie => ie.Estoque)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    ie => ie.CodigoEstoque == codigoEstoque 
                    && ie.CodigoItem == codigoItem,
                    cancellationToken);
        }

        public async Task<IEnumerable<ItemEstoque>> BuscarPorCodigoEstoque(int codigoEstoque,CancellationToken cancellationToken)
        {
            return await _context.ItensEstoque
                .Include(ie => ie.Item)
                .Include(ie => ie.Estoque)
                .AsNoTracking()
                .Where(ie => ie.CodigoEstoque == codigoEstoque)
                .OrderBy(ie => ie.CodigoItem)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ItemEstoque>> ListarAsync(CancellationToken cancellationToken)
        {
            return await _context.ItensEstoque
                .Include(ie => ie.Item)       // navegação → OK
                .Include(ie => ie.Estoque)    // navegação → OK
                .AsNoTracking()
                .OrderBy(ie => ie.CodigoEstoque)
                .ThenBy(ie => ie.CodigoItem)
                .ToListAsync(cancellationToken);
        }

        // a ideia é quando chamar a função AtualizarQuantidadeAsync já passar a quantidade correta a ser atualizada a operação fica por conta da Movimentação.
        public async Task AtualizarQuantidadeAsync(int codigoEstoque, int codigoItem, int quantidade, CancellationToken cancellationToken)
        {
            var itemEstoque = await _context.ItensEstoque
                .FirstOrDefaultAsync(ie => ie.CodigoEstoque == codigoEstoque && ie.CodigoItem == codigoItem, cancellationToken);

            if (itemEstoque != null)
            {
                itemEstoque.SetQuantidade(quantidade);
                itemEstoque.SetDataAlteracao(DateTime.UtcNow);
                _context.ItensEstoque.Update(itemEstoque);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        
    }
}
