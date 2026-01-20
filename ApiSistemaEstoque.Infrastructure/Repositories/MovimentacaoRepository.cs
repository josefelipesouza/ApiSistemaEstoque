using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        public IUnityOfWork UnitOfWork => _context;

        private readonly EstoqueContext _context;

        public MovimentacaoRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Movimentacao movimentacao, CancellationToken cancellationToken)
        {
            await _context.Movimentacoes.AddAsync(movimentacao, cancellationToken);
        }

        public async Task AdicionarItemAsync(ItemMovimentacao itemMovimentacao, CancellationToken cancellationToken)
        {
            await _context.ItensMovimentacao.AddAsync(itemMovimentacao, cancellationToken);
        }

        public async Task<IEnumerable<Movimentacao>> ListarAsync(CancellationToken cancellationToken)
        {
            return await _context.Movimentacoes
                .AsNoTracking()
                .Include(m => m.ItensMovimentacao) // Inclui os itens
                .OrderBy(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Movimentacao?> BuscarMovimentacaoPorCodigoAsync(int codigo, CancellationToken cancellationToken)
        {
            return await _context.Movimentacoes
                .AsNoTracking()
                .Include(m => m.ItensMovimentacao) // Inclui os itens
                .FirstOrDefaultAsync(m => m.Codigo == codigo, cancellationToken);
        }

        public async Task<IEnumerable<Movimentacao>> BuscarMovimentacaoAsync(
        int codigoEstoqueSolicitante,
        int codigoEstoqueSolicitado,
        int codigoTipoMovimentacao,
        DateTime dataInicial,
        DateTime dataFinal,
        CancellationToken cancellationToken)
        {
            return await _context.Movimentacoes
                .AsNoTracking()
                .Include(m => m.ItensMovimentacao) // Inclui os itens
                .Where(m =>
                    m.CodigoEstoqueSolicitante == codigoEstoqueSolicitante &&
                    m.CodigoEstoqueSolicitado == codigoEstoqueSolicitado &&
                    m.CodigoTipoMovimentacao == codigoTipoMovimentacao &&
                    m.CreatedAt.Date >= dataInicial.Date &&
                    m.CreatedAt.Date <= dataFinal.Date
                )
                .ToListAsync(cancellationToken);
        }

        public void Atualizar(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Update(movimentacao);
        }
    }
}
