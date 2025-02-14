using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class ItemEstoque
{
    [Key]
    public int Codigo { get; set; }
    public int CodigoItem { get; set; }
    public int CodigoEstoque { get; set; }
    public int Quantidade { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime updated_at { get; private set; }

    [ForeignKey(nameof(CodigoItem))]
    public Item Item { get; private set; }

    [ForeignKey(nameof(CodigoEstoque))]
    public Estoque Estoque { get; private set; }

    public ItemEstoque(int codigoItem, int codigoEstoque, int quantidade)
    {
        CodigoItem = codigoItem;
        CodigoEstoque = codigoEstoque;
        Quantidade = quantidade;
        //CreatedAt = DateTime.UtcNow;
    }

    private ItemEstoque() { }

    public void SetCodigoItem(int codigoItem)
    {
        CodigoItem = codigoItem;
    }

    public void SetCodigoEstoque(int codigoEstoque)
    {
        CodigoEstoque = codigoEstoque;
    }

    public void SetQuantidade(int quantidade)
    {
        Quantidade = quantidade;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at = dataAlteracao;
    }

}

