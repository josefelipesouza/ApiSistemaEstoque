using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Categoria
{
    [Key]
    public int Codigo { get; set; }
    public string Descricao { get; private set; }
    public int Superior { get; private set; }
    public int UsuarioCadastro { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}
    public IEnumerable<Status> Inativo { get; private set; }

    public Categoria(string descricao, int superior, int usuarioCadastro)
    {
        Descricao = descricao;
        Superior = superior;
        UsuarioCadastro = usuarioCadastro;
        CreatedAt =  DateTime.UtcNow;
        Inativo = new List<Status> { Status.Ativo };

    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetSuperior(int superior)
    {
        Superior = superior;
    }

    public void SetUsuarioCadastro(int usuarioCadastro)
    {
        UsuarioCadastro = usuarioCadastro;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at  = dataAlteracao;
    }

    public void SetInativar()
    {
        Inativo = new List<Status> { Status.Inativo };
    }
}

