using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories
{
    public class TransporteRepository : ITransporteRepository
    {
        private readonly EstoqueContext _context;

        public IUnityOfWork UnitOfWork => _context;

        public TransporteRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Transporte transporte, CancellationToken cancellationToken)
        {
            await _context.Transportes.AddAsync(transporte, cancellationToken);
        }

        public async Task<IEnumerable<Transporte>> BuscarPorCodigoMovimentacaoAsync(
            int codigoMovimentacao,
            CancellationToken cancellationToken)
        {
            return await _context.Transportes
                .AsNoTracking()
                .Where(t => t.CodigoMovimentacao == codigoMovimentacao)
                .OrderBy(t => t.CodigoItem)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transporte>> BuscarPorPlacaVeiculoAsync(
            string placaVeiculo,
            CancellationToken cancellationToken)
        {
            return await _context.Transportes
                .AsNoTracking()
                .Where(t => t.PlacaVeiculo == placaVeiculo)
                .OrderBy(t => t.CodigoMovimentacao)
                .ThenBy(t => t.CodigoItem)
                .ToListAsync(cancellationToken);
        }

        public async Task AtualizarDataEntregaAsync(
            int codigoMovimentacao,
            CancellationToken cancellationToken)
        {
            var transportes = await _context.Transportes
                .Where(t => t.CodigoMovimentacao == codigoMovimentacao)
                .ToListAsync(cancellationToken);

            if (!transportes.Any())
                return;

            foreach (var transporte in transportes)
            {
                transporte.SetDataEntrega(DateTime.UtcNow);
            }

            _context.Transportes.UpdateRange(transportes);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
