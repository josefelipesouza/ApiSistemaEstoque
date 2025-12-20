using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Item
{
    [Key]
    public int Codigo { get; set; }
    //public ICollection<ItemEstoque> ItensEstoque { get; set; } = new List<ItemEstoque>();
    public string Descricao { get; private set; }
    public int QuantidadeMinima { get; private set; }
    public string Referencia { get; private set; }
    public int CodigoCategoria { get; private set; }
    public int CodigoUnidade { get; private set; }
    public string UsuarioCadastro { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime updated_at { get; private set; }
    public Status Inativo { get; private set; }


    [ForeignKey(nameof(CodigoCategoria))]
    public Categoria Categoria { get; private set; }

    [ForeignKey(nameof(CodigoUnidade))]
    public Unidade Unidade { get; private set; }
    /*
    [ForeignKey(nameof(UsuarioCadastro))]
    public Usuario Usuario { get; private set; }
    */
    public Item(string descricao, string usuarioCadastro, int quantidadeMinima, string referencia, int codigoCategoria, int codigoUnidade)
    {
        Descricao = descricao;
        UsuarioCadastro = usuarioCadastro;
        QuantidadeMinima = quantidadeMinima;
        Referencia = referencia;
        CodigoCategoria = codigoCategoria;
        CodigoUnidade = codigoUnidade;
        CreatedAt = DateTime.UtcNow;
       Inativo = Status.Ativo;
    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetQuantidadeMinima(int quantidadeMinima)
    {
        QuantidadeMinima = quantidadeMinima;
    }

    public void SetReferencia(string referencia)
    {
        Referencia = referencia;
    }

    public void SetCodigoCategoria(int codigoCategoria)
    {
        CodigoCategoria = codigoCategoria;
    }

    public void SetCodigoUnidade(int codigoUnidade)
    {
        CodigoUnidade = codigoUnidade;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at = dataAlteracao;
    }    

    public void SetInativar()
    {
        Inativo = Status.Inativo;
    }

}

