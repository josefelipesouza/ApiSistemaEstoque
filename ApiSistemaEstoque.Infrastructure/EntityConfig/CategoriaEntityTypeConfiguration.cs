using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig
{
    public class CategoriaEntityTypeConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(c => c.Codigo);

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.UsuarioCadastro)
                .IsRequired();

            builder.Property(c => c.Superior)
                .IsRequired(false);

            // =============================
            // SELF REFERENCE (Categoria Superior)
            // =============================
            builder
                .HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(c => c.Superior)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================
            // REMOVIDO: FK PARA ASPNETUSERS
            // UsuarioCadastro agora é apenas audit field
            // =============================
        }
    }
}
