using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context
{
    public class EstoqueContext : DbContext, IUnityOfWork
    {
        public EstoqueContext(DbContextOptions<EstoqueContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Estoque> Estoques { get; set; } = null!;
        public DbSet<HistoricoValorItem> HistoricoValorItens { get; set; } = null!;
        public DbSet<Item> Itens { get; set; } = null!;
        public DbSet<ItemEstoque> ItensEstoque { get; set; } = null!;
        public DbSet<ItemMovimentacao> ItensMovimentacao { get; set; } = null!;
        public DbSet<KardexDiario> KardexDiario { get; set; } = null!;
        public DbSet<Movimentacao> Movimentacoes { get; set; } = null!;
        public DbSet<TipoMovimentacao> TiposMovimentacoes { get; set; } = null!;
        public DbSet<Unidade> Unidades { get; set; } = null!;
        public DbSet<Transporte> Transportes { get; set; } = null!;
        public DbSet<UsuarioEstoque> UsuariosEstoques { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================================
            // 🔹 ESTOQUE (CORREÇÃO DO SUPERIOR NULLABLE)
            // ============================================================
            modelBuilder.Entity<Estoque>(builder =>
            {
                builder.HasKey(e => e.Codigo);

                builder.Property(e => e.Descricao)
                    .IsRequired();

                builder.Property(e => e.UsuarioCadastro)
                    .IsRequired();

                builder.Property(e => e.Localizacao)
                    .IsRequired();

                builder.Property(e => e.Responsavel)
                    .IsRequired();

                builder.Property(e => e.Superior)
                    .IsRequired(false);

                builder.Property(e => e.CreatedAt)
                    .IsRequired();

                builder.Property(e => e.updated_at)
                    .IsRequired();

                builder.Property(e => e.Inativo)
                    .IsRequired();
            });
            // ============================================================
            // 🔹 USUÁRIOESTOQUE
            // ============================================================
            modelBuilder.Entity<UsuarioEstoque>(entity =>
            {
                entity.ToTable("UsuariosEstoques");

                entity.HasKey(x => x.Codigo);

                entity.Property(x => x.CodigoUsuario)
                    .IsRequired();

                entity.Property(x => x.CodigoEstoque)
                    .IsRequired();

                entity.HasIndex(x => new { x.CodigoUsuario, x.CodigoEstoque })
                    .IsUnique();

                entity.HasOne<Estoque>()
                    .WithMany()
                    .HasForeignKey(x => x.CodigoEstoque)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================================
            // 🔹 CATEGORIA
            // ============================================================
            modelBuilder.ApplyConfiguration(new CategoriaEntityTypeConfiguration());

            // ============================================================
            // 🔹 HISTORICO VALOR ITEM
            // ============================================================
            modelBuilder.Entity<HistoricoValorItem>()
                .HasKey(h => h.Codigo);

            // ============================================================
            // 🔹 ITEM
            // ============================================================
            modelBuilder.Entity<Item>()
                .HasKey(i => i.Codigo);

            // ============================================================
            // 🔹 ITEM ESTOQUE
            // ============================================================
            modelBuilder.Entity<ItemEstoque>(builder =>
            {
                builder.HasKey(ie => ie.Codigo);

                builder.Property(ie => ie.Codigo)
                    .ValueGeneratedOnAdd(); // 🔥 ESSENCIAL

                builder.Property(ie => ie.Quantidade)
                    .IsRequired();

                builder.HasOne(ie => ie.Item)
                    .WithMany()
                    .HasForeignKey(ie => ie.CodigoItem)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(ie => ie.Estoque)
                    .WithMany()
                    .HasForeignKey(ie => ie.CodigoEstoque)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(ie => new { ie.CodigoItem, ie.CodigoEstoque })
                    .IsUnique();
            });

            // ============================================================
            // 🔹 ITEM MOVIMENTAÇÃO
            // ============================================================
            modelBuilder.Entity<ItemMovimentacao>(builder =>
            {
                builder.HasKey(im => im.Codigo);

                builder.Property(im => im.Codigo)
                    .ValueGeneratedOnAdd();

                builder.Property(im => im.CodigoMovimentacao)
                    .HasDefaultValue(0);

                builder.Property(im => im.Item)
                    .HasDefaultValue(0);

                builder.Property(im => im.Quantidade)
                    .HasDefaultValue(1);

                builder.HasOne(im => im.Movimentacao)
                    .WithMany(m => m.ItensMovimentacao)
                    .HasForeignKey(im => im.CodigoMovimentacao)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================================
            // 🔹 KARDEX DIARIO
            // ============================================================
            modelBuilder.Entity<KardexDiario>()
                .HasKey(kd => kd.Codigo);

            // ============================================================
            // 🔹 MOVIMENTAÇÃO
            // ============================================================
            modelBuilder.Entity<Movimentacao>(builder =>
            {
                builder.HasKey(m => m.Codigo);

                builder.Property(m => m.Codigo)
                    .ValueGeneratedOnAdd();

                builder.Property(m => m.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(StatusMovimentacao.Novo);

                builder.Property(m => m.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                builder.Property(m => m.updated_at)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                builder.HasOne(m => m.TipoMovimentacao)
                    .WithMany()
                    .HasForeignKey(m => m.CodigoTipoMovimentacao)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================================
            // 🔹 TIPO MOVIMENTAÇÃO
            // ============================================================
            modelBuilder.Entity<TipoMovimentacao>()
                .HasKey(tm => tm.Codigo);

            // ============================================================
            // 🔹 UNIDADE
            // ============================================================
            modelBuilder.Entity<Unidade>()
                .HasKey(u => u.Codigo);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
