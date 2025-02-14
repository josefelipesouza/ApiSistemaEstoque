using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class ItemMovimentacao
{
  [Key]
  public int? Codigo { get; set; }
  public int? CodigoMovimentacao { get; set; }
  public int? Item { get; set; }
  public int? Quantidade { get; set; }

  [ForeignKey(nameof(CodigoMovimentacao))]
  public Movimentacao Movimentacao { get; private set; }

  public ItemMovimentacao() { }

  public ItemMovimentacao(int? codigoMovimentacao, int? item, int? quantidade)
  {
    CodigoMovimentacao = codigoMovimentacao;
    Item = item;
    Quantidade = quantidade;

  }

}

