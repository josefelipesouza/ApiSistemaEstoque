using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig
{
    public class CategoriaEntityTypeConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Codigo);
            builder.Property(c => c.Descricao).IsRequired();
            builder.Property(c => c.UsuarioCadastro).IsRequired();

            builder
                .HasOne<IdentityUser>()              // Entidade de destino
                .WithMany()                          // IdentityUser não tem coleção
                .HasForeignKey(c => c.UsuarioCadastro)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
