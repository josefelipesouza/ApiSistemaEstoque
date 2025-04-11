using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;

public class EstoqueContext : DbContext, IUnityOfWork
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<HistoricoValorItem> HistoricoValorItens { get; set; }
    public DbSet<Item> Itens { get; set; }
    public DbSet<ItemEstoque> ItensEstoque { get; set; }
    public DbSet<ItemMovimentacao> ItensMovimentacao { get; set; }
    public DbSet<KardexDiario> KardexDiario { get; set; }
    public DbSet<Movimentacao> Movimentacoes { get; set; }
    public DbSet<TipoMovimentacao> TiposMovimentacoes { get; set; }
    public DbSet<Unidade> Unidades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite(@"Data Source=C:\Users\User\Desktop\dev\.net\ApiSistemaEstoque\Banco.sqlite");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Categoria
        modelBuilder.Entity<Categoria>()
            .HasKey(c => c.Codigo);

        // Configuração da entidade Estoque
        modelBuilder.Entity<Estoque>()
            .HasKey(e => e.Codigo);

        // Configuração da entidade HistoricoValorItem
        modelBuilder.Entity<HistoricoValorItem>()
            .HasKey(h => h.Codigo);

        // Configuração da entidade Item
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Codigo);

        modelBuilder.Entity<ItemEstoque>()
    .HasKey(ie => new { ie.CodigoItem, ie.CodigoEstoque }); // Chave composta

        modelBuilder.Entity<ItemEstoque>()
            .Property(ie => ie.Quantidade)
            .IsRequired();

        modelBuilder.Entity<ItemEstoque>()
            .HasOne(ie => ie.Item)
            .WithMany()
            .HasForeignKey(ie => ie.CodigoItem)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemEstoque>()
            .HasOne(ie => ie.Estoque)
            .WithMany()
            .HasForeignKey(ie => ie.CodigoEstoque)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurando o construtor
        modelBuilder.Entity<ItemEstoque>()
            .HasKey(ie => new { ie.CodigoItem, ie.CodigoEstoque });
        modelBuilder.Entity<ItemEstoque>()
            .Property(ie => ie.CodigoItem)
            .HasDefaultValue(0);

        modelBuilder.Entity<ItemEstoque>()
            .Property(ie => ie.CodigoEstoque)
            .HasDefaultValue(0);

        modelBuilder.Entity<ItemEstoque>()
            .Property(ie => ie.Quantidade)
            .HasDefaultValue(0);

        // Configuração de chave composta
        modelBuilder.Entity<ItemMovimentacao>()
            .HasKey(im => new { im.CodigoMovimentacao, im.Item }); // Chave composta

        // Configuração de propriedades com valor padrão
        modelBuilder.Entity<ItemMovimentacao>()
            .Property(im => im.CodigoMovimentacao)
            .HasDefaultValue(0);

        modelBuilder.Entity<ItemMovimentacao>()
            .Property(im => im.Item)
            .HasDefaultValue(0);

        modelBuilder.Entity<ItemMovimentacao>()
            .Property(im => im.Quantidade)
            .HasDefaultValue(1);

        // Configuração de relacionamento
        modelBuilder.Entity<ItemMovimentacao>()
            .HasOne(im => im.Movimentacao)
            .WithMany(m => m.ItensMovimentacao)
            .HasForeignKey(im => im.CodigoMovimentacao)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração da entidade KardexDiario
        modelBuilder.Entity<KardexDiario>()
            .HasKey(kd => kd.Codigo);

        modelBuilder.Entity<Movimentacao>()
    .HasKey(m => m.Codigo); // Define a chave primária

        modelBuilder.Entity<Movimentacao>()
            .Property(m => m.Codigo)
            .ValueGeneratedOnAdd(); // Configura geração automática de ID

        // Configuração de relacionamentos e propriedades adicionais
        modelBuilder.Entity<Movimentacao>()
            .Property(m => m.Status)
            .HasConversion<string>() // Converte enums para string no banco de dados
            .HasDefaultValue(StatusMovimentacao.Aguardando);

        modelBuilder.Entity<Movimentacao>()
            .Property(m => m.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Define valor padrão para data de criação

        modelBuilder.Entity<Movimentacao>()
            .Property(m => m.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Define valor padrão para data de atualização

        // Configuração de relacionamento com TipoMovimentacao
        modelBuilder.Entity<Movimentacao>()
            .HasOne(m => m.TipoMovimentacao)
            .WithMany()
            .HasForeignKey(m => m.CodigoTipoMovimentacao)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração de relacionamento com ItemMovimentacao
        modelBuilder.Entity<Movimentacao>()
            .HasMany(m => m.ItensMovimentacao)
            .WithOne(im => im.Movimentacao)
            .HasForeignKey(im => im.CodigoMovimentacao)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Codigo);

            entity.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(150);

            // Relacionamento com Setor
            entity.HasOne(u => u.Setor)
                .WithMany(s => s.Usuarios)  // Agora 'Usuarios' existe em Setor
                .HasForeignKey(u => u.CodigoSetor)
                .OnDelete(DeleteBehavior.Restrict); // Define comportamento de deleção
        });

        modelBuilder.Entity<Setor>(entity =>
        {
            entity.HasKey(s => s.Codigo);

            entity.Property(s => s.Descricao)
                  .IsRequired()
                  .HasMaxLength(200);

            // Não é necessário duplicar a configuração do relacionamento
        });

        // Configuração da entidade TipoMovimentacao
        modelBuilder.Entity<TipoMovimentacao>()
            .HasKey(tm => tm.Codigo);

        // Configuração da entidade Unidade
        modelBuilder.Entity<Unidade>()
            .HasKey(u => u.Codigo);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
