using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Categoria
{
    private string usuarioCadastro;

    [Key]
    public int Codigo { get; set; }
    public string Descricao { get; private set; }
    public int Superior { get; private set; }
    public String UsuarioCadastro { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}
    public Status Inativo { get; private set; }
    
    public IdentityUser Usuario { get; set; }


    public Categoria(string descricao, int superior, string usuarioCadastro)
    {
        Descricao = descricao;
        Superior = superior;
        UsuarioCadastro = usuarioCadastro;
        CreatedAt = DateTime.UtcNow;
        Inativo = Status.Ativo;

    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetSuperior(int superior)
    {
        Superior = superior;
    }

    public void SetUsuarioCadastro(string usuarioCadastro)
    {
        UsuarioCadastro = usuarioCadastro;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at  = dataAlteracao;
    }

    public void SetInativar()
    {
        Inativo = Status.Inativo;
    }
}

