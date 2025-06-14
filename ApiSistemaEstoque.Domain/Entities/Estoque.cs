using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Estoque
{
    [Key]
    public int Codigo { get; set; }
    public ICollection<ItemEstoque> ItensEstoque { get; set; } = new List<ItemEstoque>();
    public string Descricao { get; private set; }
    public string UsuarioCadastro { get; private set; }
    public string Localizacao { get; private set; }
    public int Responsavel { get; private set; }
    public int Superior { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}
    public IEnumerable<Status> Inativo { get; private set; }

    public Estoque(string descricao, string usuarioCadastro, string localizacao, int responsavel, int superior)
    {
        Descricao = descricao;
        UsuarioCadastro = usuarioCadastro;
        Localizacao = localizacao;
        Responsavel = responsavel;
        Superior = superior;
        CreatedAt =  DateTime.UtcNow;
        Inativo = new List<Status> { Status.Ativo };
    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetLocalizacao(string localizacao)
    {
        Localizacao = localizacao;
    }

    public void SetResponsavel(int responsavel)
    {
        Responsavel = responsavel;
    }

    public void SetSuperior(int superior)
    {
        Superior = superior;
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

