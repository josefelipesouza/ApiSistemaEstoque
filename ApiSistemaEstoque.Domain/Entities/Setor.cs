using System.ComponentModel.DataAnnotations;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Setor
{
    [Key]
    public int Codigo { get; private set; }
    public string Descricao { get; private set; }
    public string UsuarioCadastro { get; private set; }
    // Propriedade de navegação para os usuários associados a este setor
    //public ICollection<Usuario> Usuarios { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}
    public Status Inativo { get; private set; }

    /*
    [ForeignKey(nameof(UsuarioCadastro))]
    public Usuario Usuario { get; private set; }
    */
    public Setor(string descricao, string usuarioCadastro)
    {
        Descricao = descricao;
        UsuarioCadastro = usuarioCadastro;
        //Usuarios = new List<Usuario>();  // Inicializando a coleção de usuários
        CreatedAt =  DateTime.UtcNow;
        Inativo = Status.Ativo;
    }

}