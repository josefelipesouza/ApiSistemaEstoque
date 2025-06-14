using System.ComponentModel.DataAnnotations;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Unidade
{
    [Key]
    public int Codigo { get; set; }

    public string Descricao { get; private set; }

    public string UsuarioCadastro { get; private set; }

    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}

    public IEnumerable<Status> Inativo { get; private set; }


    public Unidade(string descricao, string usuarioCadastro)
    {
        Descricao = descricao;
        UsuarioCadastro = usuarioCadastro;
        CreatedAt =  DateTime.UtcNow;
        Inativo = new List<Status> { Status.Ativo };
    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at = dataAlteracao;
    }

    public void SetInativar()
    {
        Inativo = new List<Status> { Status.Inativo };
    }
}

