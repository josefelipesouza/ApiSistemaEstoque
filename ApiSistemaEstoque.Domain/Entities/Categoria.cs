using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities
{
    public class Categoria
    {
        [Key]
        public int Codigo { get; private set; }

        [Required]
        public string Descricao { get; private set; }

        public int Superior { get; private set; }

        [Required]
        public string UsuarioCadastro { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime updated_at { get; private set; }

        public Status Inativo { get; private set; }

        /*
        // Navegação para o IdentityUser (usuário que cadastrou)
        [ForeignKey(nameof(UsuarioCadastro))]
        public IdentityUser Usuario { get; private set; }
        */

        // Construtor para uso na criação da categoria
        public Categoria(string descricao, int superior, string usuarioCadastro)
        {
            Descricao = descricao;
            Superior = superior;
            UsuarioCadastro = usuarioCadastro;
            CreatedAt = DateTime.UtcNow;
            Inativo = Status.Ativo;
        }

        // Construtor sem parâmetros necessário para o EF Core
        protected Categoria() { }

        // Métodos para alterar propriedades
        public void SetDescricao(string descricao) => Descricao = descricao;

        public void SetSuperior(int superior) => Superior = superior;

        public void SetUsuarioCadastro(string usuarioCadastro) => UsuarioCadastro = usuarioCadastro;

        public void SetDataAlteracao(DateTime dataAlteracao) => updated_at = dataAlteracao;

        public void SetInativar() => Inativo = Status.Inativo;
    }
}
