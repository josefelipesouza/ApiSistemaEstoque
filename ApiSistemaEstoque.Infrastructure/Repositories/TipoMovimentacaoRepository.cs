using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

public class TipoMovimentacaoRepository : ITipoMovimentacaoRepository
{
    private readonly EstoqueContext _context;

    public TipoMovimentacaoRepository(EstoqueContext context)
    {
        _context = context;
    }

    public IUnityOfWork UnitOfWork => _context;

    public async Task AdicionarAsync(TipoMovimentacao tipoMovimentacao, CancellationToken cancellationToken)
    {
        await _context.TiposMovimentacoes.AddAsync(tipoMovimentacao, cancellationToken);
    }

    public async Task<TipoMovimentacao?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken)
    {
        return await _context.TiposMovimentacoes
            .AsNoTracking()
            .Where(x => x.Codigo == codigo)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TipoMovimentacao>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _context.TiposMovimentacoes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Atualizar(TipoMovimentacao tipoMovimentacao)
    {
        _context.TiposMovimentacoes.Update(tipoMovimentacao);
    }
}
