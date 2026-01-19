using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(EstoqueContext context)
        {
            if (await context.TiposMovimentacoes.AnyAsync())
                return;

            const string usuarioSistema = "SYSTEM";

            var tipos = new List<TipoMovimentacao>
            {
                new(TipoBaseMovimentacao.Entrada, usuarioSistema),
                new(TipoBaseMovimentacao.CorreçãoEntrada, usuarioSistema),
                new(TipoBaseMovimentacao.Saida, usuarioSistema),
                new(TipoBaseMovimentacao.CorreçãoSaida, usuarioSistema),
                new(TipoBaseMovimentacao.Transferencia, usuarioSistema),
                new(TipoBaseMovimentacao.Solicitacao, usuarioSistema),
                new(TipoBaseMovimentacao.Devolucao, usuarioSistema)
            };

            context.TiposMovimentacoes.AddRange(tipos);
            await context.SaveChangesAsync();
        }
    }
}
